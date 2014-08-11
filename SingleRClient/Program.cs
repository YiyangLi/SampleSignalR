using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleRClient
{
    class Program
    {
        private const string ServiceUri = "http://localhost:5050/hackathon";
        static void Main(string[] args)
        {
            var connection = new Connection(ServiceUri, "name=DotNetClient");
            string inputMsg;
            connection.Received += connection_Received;
            connection.StateChanged += connection_StateChanged;

            connection.Start().Wait();
            while (!(inputMsg = Console.ReadLine()).ToUpper().Equals("Q"))
            {
                int number = 100;
                if (!int.TryParse(inputMsg, out number))
                {
                    number = 100;
                }
                List<int> tasks = new List<int>(Enumerable.Range(1, number));
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = 10;
                Parallel.ForEach(tasks, options, t =>{
                    connection.Send(string.Format("{0} : {1}", t, Guid.NewGuid()));
                    System.Threading.Thread.Sleep(100);
                });
            }
        }
        static void connection_StateChanged(StateChange state)
        {
#if DEBUG
            if (state.NewState == ConnectionState.Connected)
            {
                Console.WriteLine("Connected.");
            }
#endif
        }

        static void connection_Received(string data)
        {
#if DEBUG
            Console.WriteLine(data);
#endif
        }
    }
}
