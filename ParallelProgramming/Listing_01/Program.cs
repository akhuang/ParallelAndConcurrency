using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Listing_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<bool> t = Task.Factory.StartNew<bool>((obj) =>
            {
                Console.WriteLine("hello world");
                Console.WriteLine(obj);
                return true;
            }, 10000);

            var result = t.Result;
            Console.WriteLine("Main method complete. Press enter to finish.");

            Console.WriteLine("===========================================");

            //task canceld
            CancellationTokenSource tokenSouce = new CancellationTokenSource();
            CancellationToken token = tokenSouce.Token;
            //同一个token可用于创建多个task, 所以token.Cancel()将cancel多个任务

            //当token调用cancel方法时将调用此委托
            token.Register(() =>
            {
                Console.WriteLine(">>>> Delegate Invoked\n");
            });

            Task cancellabledTask = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected.");
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);

            Console.WriteLine("Press enter to start task ");
            Console.WriteLine("Press enter to cancel task");
            Console.ReadLine();

            cancellabledTask.Start();

            Console.ReadLine();

            Console.WriteLine("Cancelling task.");

            tokenSouce.Cancel();

            //token.WaitHandle.WaitOne();此方法将一阻塞下面的方法，直到token的Cancel方法被调用


            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");

            Console.ReadKey();
        }
    }
}
