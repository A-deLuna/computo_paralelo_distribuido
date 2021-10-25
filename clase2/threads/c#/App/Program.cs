using System;
using System.Threading;

namespace NetCore.Docker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting thread");
            CancellationTokenSource source = new CancellationTokenSource();

            Thread t = new Thread(new ThreadStart(new App(source.Token).Loop));
            t.Start();
            Thread.Sleep(3000);
            source.Cancel();
            Console.WriteLine("finished timeout");
        }
    }
    class App 
    {
      private CancellationToken cancellationToken;

      public App(CancellationToken cancellationToken) 
      {
          this.cancellationToken = cancellationToken;
      }

      public void Loop()
      {
        try {
            while(true) {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine("Hello World!");
                Thread.Sleep(100);
            }
        } catch(OperationCanceledException) {  }
      }
    }
}
