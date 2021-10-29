using Newtonsoft.Json;
using Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DBLayer
{
    
    public class Setter
    {
        public string SourceType { get; }
        public Setter(CrawlableSource source)
        {
            SourceType = source.GetType().Name;
            Source = source;
        }

        public CrawlableSource Source { get; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Source);
        }

        public PageArchitectureSite ToSource(string json)
        {
            return JsonConvert.DeserializeObject<PageArchitectureSite>(json);
        }
    }

    public class Person
    {
        public static int IdCounter = 0;
        public int Age, Height, Weight;
        public string Name;
        public Person? Parent;
        public int Id;

        public Person(string name, int age, int height, int weight, Person? parent)
        {
            Id = IdCounter++;
            Name = name;
            Age = age;
            Height = height;
            Weight = weight;
            Parent = parent;
        }
    }

    class Program
    {
       

        static void Main(string[] args)
        {
            var t = typeof(Person);

            var sv = t.GetFields();

            Console.WriteLine(sv.Length);

            foreach (var f in sv)
                Console.WriteLine(f.Name);
        }

        

    }

    internal class Spectacle
    {
        private List<Action> stack;
        public Spectacle()
        {
            stack = new List<Action>();
        }

        internal Spectacle Delay(TimeSpan timeSpan)
        {
            stack.Add(()=>Thread.Sleep(timeSpan));
            return this;
        }

        internal Spectacle Say(string v)
        {
            stack.Add(()=>Console.WriteLine(v));
            return this;
        }
        internal void Play()
        {
            foreach (var action in stack)
                action();
        }

        internal Spectacle UntilKeyPressed(Func<Spectacle, Spectacle> p)
        {
            var spectacle = p(new Spectacle());
            stack.Add
                (
                    () => 
                    {
                        while (!Console.KeyAvailable)
                        {
                            spectacle.Play();
                        }
                        Console.ReadKey(true);
                    }
                );
            return this;
        }
    }
}
