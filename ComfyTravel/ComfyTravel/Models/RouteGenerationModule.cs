using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComfyTravel.Models
{
    public class RouteGenerationModule
    {
        public static Placement getById(int id, List<Placement> places)
        {
            foreach (Placement place in places)
            {
                if (place.id == id)
                    return place;
            }
            return null;
        }

        public static List<string> MainGenerate(
            List<Objects> allPlacesParameter, List<Objects> mustHavePlacesParameter, 
            List<bool> TypesPMBCS, bool withChildren,
            DateTime start, TimeSpan duration, string transport)
        {
            List<Placement> pp = new List<Placement>();

            foreach (Objects place in allPlacesParameter)
            {
                pp.Add(new Placement(place));
            }

            // дальше параметры от пользователя
            List<Placement> MustHavePlaces = new List<Placement>();

            foreach (Objects place in mustHavePlacesParameter)
            {
                MustHavePlaces.Add(new Placement(place));
            }

            return RouteGeneration(pp, MustHavePlaces, TypesPMBCS, withChildren, start, duration, transport);
        }

        public static List<string> RouteGeneration(List<Placement> AllPlaces, List<Placement> MustHavePlaces,
                                           List<bool> TypesPMBCS, bool withChildren, DateTime startTime, TimeSpan duration,
                                           String transport)
        {
            //(доп)мб плюсом добавить проверку по времени, успевают ли добраться до всех обязательных мест

            List<Placement> TopPlaces = new List<Placement>();

            // сначала удаляем все обязательные места из всех, чтобы больше их не рассматривать
            for (int i = 0; i < AllPlaces.Count(); i++)
            {
                if (!(AllPlaces[i].isInList(MustHavePlaces)))
                    TopPlaces.Add(AllPlaces[i]);
            }
            AllPlaces = new List<Placement>(TopPlaces);
            TopPlaces.Clear();

            int notLateCount = AllPlaces.Count();
            // потом выкидываем в конец то куда уже точно не успеваем до закрытия или еще не открылось
            for (int i = 0; i < AllPlaces.Count(); i++)
            {
                if (startTime.TimeOfDay.CompareTo(AllPlaces[i].closeTime) <= 0 &&
                    startTime.TimeOfDay.CompareTo(AllPlaces[i].openTime) > 0)
                    TopPlaces.Add(AllPlaces[i]);
            }
            notLateCount = TopPlaces.Count();
            for (int i = 0; i < AllPlaces.Count(); i++)
            {
                if (startTime.TimeOfDay.CompareTo(AllPlaces[i].closeTime) > 0 ||
                    startTime.TimeOfDay.CompareTo(AllPlaces[i].openTime) <= 0)
                    TopPlaces.Add(AllPlaces[i]);
            }

            AllPlaces = new List<Placement>(TopPlaces);
            TopPlaces.Clear();
            int fits = notLateCount;

            // дальше в конец то куда нельзя с детьми если в параметрах указаны дети
            if (withChildren)
            {
                for (int i = 0; i < notLateCount; i++)
                {
                    if (AllPlaces[i].withChildren)
                        TopPlaces.Add(AllPlaces[i]);
                }
                fits = TopPlaces.Count();
                for (int i = 0; i < notLateCount; i++)
                {
                    if (!(AllPlaces[i].withChildren))
                        TopPlaces.Add(AllPlaces[i]);
                }
                for (int i = notLateCount; i < AllPlaces.Count(); i++)
                    TopPlaces.Add(AllPlaces[i]);
                AllPlaces = new List<Placement>(TopPlaces);
                TopPlaces.Clear();
            }

            //List<String> allTypes = new List<String> { "p", "m", "b", "c", "s" };
            List<String> allTypes = new List<String> 
            { 
                TypesOfObjects.Park,
                TypesOfObjects.Museum,
                TypesOfObjects.Boulevard,
                TypesOfObjects.Cinema,
                TypesOfObjects.Monument
            };

            // дальше выстраивание приоритетов по типам. сначала идут то что тру
            List<String> priority = new List<String>();
            int j = 0;
            foreach (String type in allTypes)
            {
                if (TypesPMBCS[j++])
                    priority.Add(type);
            }

            //кумовство среди типов - парки, памятники, бульвары
            //List<String> pbs = new List<String> { "p", "b", "s" };
            List<String> pbs = new List<String>
            {
                TypesOfObjects.Park,
                TypesOfObjects.Boulevard,
                TypesOfObjects.Monument
            };

            for (int i = 0; i < 3; i++)
            {
                if (priority.Contains(pbs[i]))
                {
                    if (!priority.Contains(pbs[(i + 1) % 3]))
                        priority.Add(pbs[(i + 1) % 3]);
                    if (!priority.Contains(pbs[(i + 2) % 3]))
                        priority.Add(pbs[(i + 2) % 3]);
                }
            }

            //если остались недобавленные типы, добавить в конец
            foreach (string type in allTypes)
            {
                if (!priority.Contains(type))
                    priority.Add(type);
            }

            //добавляем в порядке приоритета типов среди подходящих мест
            foreach (String p in priority)
            {
                for (int i = 0; i < fits; i++)
                {
                    if (AllPlaces[i].types == p)
                        TopPlaces.Add(AllPlaces[i]);
                }
            }
            Random rng = new Random();
            TopPlaces = TopPlaces.OrderBy(a => rng.Next()).ToList();

            //ниже можно удалить
            foreach (String p in priority)
            {
                for (int i = fits; i < notLateCount; i++)
                {
                    if (AllPlaces[i].types == p)
                        TopPlaces.Add(AllPlaces[i]);
                }
            }
            
            foreach (String p in priority)
            {
                for (int i = notLateCount; i < AllPlaces.Count(); i++)
                {
                    if (AllPlaces[i].types == p)
                        TopPlaces.Add(AllPlaces[i]);
                }
            }

            AllPlaces = new List<Placement>(TopPlaces);
            TopPlaces.Clear();

            //Console.WriteLine("\nОбязательные места:");
            //for (int i = 0; i < MustHavePlaces.Count(); i++)
            //{
            //    Console.Write(MustHavePlaces[i].id.ToString() + ' ');
            //}
            //Console.WriteLine("\nПриоритет мест такой:");
            //for (int i = 0; i < AllPlaces.Count(); i++)
            //{
            //    Console.Write(AllPlaces[i].id.ToString() + ' ');
            //}
            //Console.Write("\nиз них подходят полностью:\n");
            //for (int i = 0; i < fits; i++)
            //{
            //    Console.Write(AllPlaces[i].id.ToString() + '(' + AllPlaces[i].timein.ToString() + "), ");
            //}
            //Console.WriteLine("\nМежду ними расстояние и время:");
            //for (int i = 0; i < fits - 1; i++)
            //{
            //    for (j = i + 1; j < fits; j++)
            //    {
            //        Console.Write(AllPlaces[i].id.ToString() + " and " + AllPlaces[j].id.ToString() + " - " +
            //            AllPlaces[i].DistanceTo(AllPlaces[j]).ToString() + "km, minutes:" + (60 * AllPlaces[i].TimeTo(AllPlaces[j], transport)).ToString() + '\n');
            //    }
            //}

            Tuple<List<Placement>, double> newres = RouteGeneration(MustHavePlaces, AllPlaces, transport, duration, fits);
            List<Placement> res = newres.Item1;
            //string output = "";

            //output += "\nОбязательные места:";
            //for (int i = 0; i < MustHavePlaces.Count(); i++)
            //{
            //    output += MustHavePlaces[i].name + ' ';
            //}
            //output += "\nПриоритет мест такой:";
            //for (int i = 0; i < AllPlaces.Count(); i++)
            //{
            //    output += AllPlaces[i].types + ' ';
            //}

            string points_x = String.Join(", ", res.Select(x => x.x).ToList());
            string points_y = String.Join(", ", res.Select(x => x.y).ToList());
            string point_names = String.Join(" -> ", res.Select(x => x.name).ToList());

            //output += "\nПримерный маршрут такой:";
            //for (int i = 0; i < res.Count(); i++)
            //    output += res[i].name + '(' + res[i].timein.ToString() + "), ";
            //output += "\nМежду ними расстояние и время:";
            //for (int i = 0; i < res.Count() - 1; i++)
            //    output += res[i].id.ToString() + " and " + res[i + 1].id.ToString() + " - " 
            //        + res[i].DistanceTo(res[i + 1]).ToString() 
            //        + "km, minutes:" + (60 * res[i].TimeTo(res[i + 1], transport)).ToString() 
            //        + '\n';

            //output += "Вы просили - " + duration.ToString() + ", мы сделали - " + newres.Item2.ToString();

            //return output;
            return new List<string>() { points_x, points_y, point_names };

            //Console.WriteLine("Примерный маршрут такой:");
            //for (int i = 0; i < res.Count(); i++)
            //    Console.Write(res[i].id.ToString() + '(' + res[i].timein.ToString() + "), ");
            //Console.WriteLine("\nМежду ними расстояние и время:");
            //for (int i = 0; i < res.Count() - 1; i++)
            //    Console.Write(res[i].id.ToString() + " and " + res[i + 1].id.ToString() + " - " +
            //            res[i].DistanceTo(res[i + 1]).ToString() + "km, minutes:" + (60 * res[i].TimeTo(res[i + 1], transport)).ToString() + '\n');
            //Console.WriteLine("Вы просили - " + duration.ToString() + ", мы сделали - " + newres.Item2.ToString());
        }

        public static Tuple<List<Placement>, double> RouteGeneration(List<Placement> AllPlaces, List<Placement> OtherPlaces, String transport, TimeSpan duration, int fits)
        {
            double askedTime = duration.Hours + (duration.Minutes / 60);
            Tuple<List<Placement>, double> res = MinRoute(AllPlaces, OtherPlaces, transport, duration);
            int l = 0;
            Tuple<List<Placement>, double> newres = new Tuple<List<Placement>, double>(res.Item1, res.Item2);
            if (AllPlaces.Count() == 0)
                newres = new Tuple<List<Placement>, double>(null, 0);
            if (AllPlaces.Count() == 1)
                newres = new Tuple<List<Placement>, double>(AllPlaces, AllPlaces[0].timein);
            /*while (newres.Item2 < askedTime - 0.5)
            {
                List<Placement> newAllPlaces = new List<Placement>(AllPlaces);
                newAllPlaces.Add(OtherPlaces[l]);
                newres = MinRoute(newAllPlaces, OtherPlaces, transport, duration);
                l = l + 1;
                newAllPlaces.Remove(OtherPlaces[l - 1]);
            }*/
            bool t = true;
            List<Placement> newAllPlaces = new List<Placement>(AllPlaces);
            while (t)
            {
                newAllPlaces.Add(OtherPlaces[l]);
                newres = MinRoute(newAllPlaces, OtherPlaces, transport, duration);
                if (newres.Item2 <= askedTime + 0.2)
                    if (newres.Item2 > askedTime - 0.5)
                        return newres;
                if (newres.Item2 > askedTime + 0.2)
                {
                    newAllPlaces.Remove(OtherPlaces[l]);
                    OtherPlaces.RemoveAt(l);
                }
                l++;

                if (l >= fits - 1)
                    break;
            }

            if (newres.Item2 <= askedTime)
                if (newres.Item2 > askedTime - 0.5)
                    return newres;
            t = true;
            l = fits;
            while (t)
            {
                newAllPlaces.Add(OtherPlaces[l]);
                newres = MinRoute(newAllPlaces, OtherPlaces, transport, duration);
                if (newres.Item2 <= askedTime & newres.Item2 > askedTime - 0.5)
                    t = false;
                else
                {
                    if (newres.Item2 > askedTime)
                    {
                        newAllPlaces.Remove(OtherPlaces[l]);
                        OtherPlaces.RemoveAt(l);
                    }
                    l++;

                }
                for (int i = 0; i < newres.Item1.Count(); i++)
                    Console.Write(newres.Item1[i].id.ToString());
                if (l >= OtherPlaces.Count())
                    break;
            }
            return newres;
        }

        public static Tuple<List<Placement>, double> MinRoute(List<Placement> AllPlaces, List<Placement> OtherPlaces, String transport, TimeSpan duration)
        {
            Dictionary<string, double> times = new Dictionary<string, double>(5);

            times[TypesOfObjects.Park] = 0.5; 
            times[TypesOfObjects.Museum] = 1; 
            times[TypesOfObjects.Cinema] = 1.75; 
            times[TypesOfObjects.Boulevard] = 0.25; 
            times[TypesOfObjects.Monument] = 0.2;

            int n = AllPlaces.Count();
            double[][] Time = new double[n][];
            for (int i = 0; i < n; i++)
                Time[i] = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                        Time[i][j] = 100000;
                    else if (i > j)
                        Time[i][j] = Time[j][i];
                    else
                    {
                        Time[i][j] = AllPlaces[i].TimeTo(AllPlaces[j], transport);
                    }
                }
            }

            List<Placement> order = new List<Placement>();
            bool[] used = new bool[n];
            List<Placement> resOrder = new List<Placement>();
            double resminTime = 100000;
            int i1, i2;

            for (int k = 0; k < n; k++)
                used[k] = false;
            for (int i = 0; i < n; i++)
            {
                i1 = i;
                order.Add(AllPlaces[i1]);
                used[i1] = true;
                double minTime = times[AllPlaces[i1].types];
                while (order.Count() != n)
                {
                    i2 = MinDistance(Time[i1], used);
                    minTime += Time[i1][i2] + times[AllPlaces[i2].types];
                    order.Add(AllPlaces[i2]);
                    used[i2] = true;
                    i1 = i2;
                }
                if (minTime < resminTime)
                {
                    resminTime = minTime;
                    resOrder.Clear();
                    resOrder = new List<Placement>(order);
                }
                order.Clear();
                for (int k = 0; k < n; k++)
                    used[k] = false;

            }
            // resorder - кратчайший путь между всеми обязательными точками

            // надо обработать если уже не умещаемся по времени - вывести уведомление об этом
            return (new Tuple<List<Placement>, double>(resOrder, resminTime));
        }

        public static int MinDistance(double[] Time, bool[] used)
        {
            double res = 10000000;
            int minarg = -1;
            for (int i = 0; i < Time.Count(); i++)
                for (int j = 0; j < Time.Count(); j++)
                    if (Time[j] < res)
                    {
                        if (used[j] == false)
                        {
                            res = Time[j];
                            minarg = j;
                        }
                    }
            return (minarg);
        }
    }

    public class Placement
    {
        public int id { get; set; }

        public string name;

        public string types;

        public bool withChildren { get; set; }

        public TimeSpan openTime, closeTime;

        public double x, y;

        public double timein { get; set; }

        public Placement(Objects place)
        {
            this.id = place.Id;
            this.name = place.Name;
            this.types = place.Type;
            this.withChildren = place.Kids;
            this.x = place.Coordinates.Item1;
            this.y = place.Coordinates.Item2;
            
            switch (types)
            {
                case TypesOfObjects.Museum:
                    this.closeTime = new TimeSpan(17, 0, 0);
                    this.openTime = new TimeSpan(10, 0, 0);
                    this.timein = 1;
                    break;
                case TypesOfObjects.Cinema:
                    this.closeTime = new TimeSpan(22, 0, 0);
                    this.openTime = new TimeSpan(9, 0, 0);
                    this.timein = 1.75;
                    break;
                case TypesOfObjects.Boulevard:
                    this.closeTime = new TimeSpan(23, 59, 59);
                    this.openTime = new TimeSpan(0, 0, 0);
                    this.timein = 0.25;
                    break;
                case TypesOfObjects.Monument:
                    this.closeTime = new TimeSpan(23, 59, 59);
                    this.openTime = new TimeSpan(0, 0, 0);
                    this.timein = 0.2;
                    break;
                case TypesOfObjects.Park:
                    this.closeTime = new TimeSpan(23, 59, 59);
                    this.openTime = new TimeSpan(0, 0, 0);
                    this.timein = 0.5;
                    break;
                default:
                    this.closeTime = new TimeSpan(23, 59, 59);
                    this.openTime = new TimeSpan(0, 0, 0);
                    break;
            }
        }

        public bool isInList(List<Placement> places)
        {
            foreach (Placement place in places)
            {
                if (this.id == place.id)
                    return true;
            }
            return false;
        }

        public double DistanceTo(Placement p)
        {
            double x2 = p.x;
            double y2 = p.y;
            double x1 = this.x;
            double y1 = this.y;
            return (111.2 * Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * Math.Cos(Math.PI * x1 / 180) * (y1 - y2) * Math.Cos(Math.PI * x1 / 180)));
        }

        public double TimeTo(Placement p, String transport)
        {
            if (this.DistanceTo(p) == 0)
                return 0;
            switch (transport)
            {
                case (TypesOfTransport.Walk):
                    return (this.DistanceTo(p) / 4 * 3 / 2); // при скорость 4 км\ч и умножить на полтора для погрешности
                case (TypesOfTransport.Car):
                    return (this.DistanceTo(p) / 60 * 3 / 2);
                case (TypesOfTransport.Public):
                    return (this.DistanceTo(p) / 60 * 3 / 2 + 0.5); // тут еще двадцать минут на путь доъот остановки

            }
            return 0;
        }
    }


}
