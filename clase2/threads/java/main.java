public class main {
  public static void main(String[] args) throws Exception {
    Looper l = new Looper();
    Thread t = new Thread(l);
    t.start();
    Thread.sleep(3000);
    l.stop();
  }

  static class Looper implements Runnable {
    private volatile boolean running = true;

    public void stop() {
      running = false;
    }
    public void run() {
      try {
        while(running) {
          System.out.println("hello, world");
          Thread.sleep(100);
        }
      } catch(InterruptedException e){}
    }
  }
}
