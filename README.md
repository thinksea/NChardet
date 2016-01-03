# NChardet
.NET自动字符编码识别程序库 NChardet

# 什么是NChardet
NChardet是mozilla自动字符编码识别程序库chardet的.NET实现，它移植自jchardet，chardet的java版实现，可实现对给定字符流的编码探测。

# NChardet是如何工作的

NChardet通过逐个比较输入字符来猜测编码；由于是猜测，所以可能会有不能完全识别的情况；如果输入字符不能确定正确的编码，那么NChardet会给出一组可能的编码值。

# 如何使用NChardet

要使用NChardet来探测编码，需要进行如下步骤。

1. 使用制定的语言线索来构造Detector类的实例对象。
2. 用实现了ICharsetDetectionObserver接口的对象作为参数来调用Detector类的Init方法。
3. 传入要探测的字符流进行编码探测。
4. 调用Detector类的DataEnd方法。
5. 得到结果或可能的结果集。

语言线索是一个整数，可用的语言线索有如下几个：

1. Japanese
2. Chinese 
3. Simplified Chinese 
4. Traditional Chinese 
5. Korean 
6. Dont know (默认)
ICharsetDetectionObserver接口只有一个Notify方法，当NChardet引擎认为自己已经探测出正确的编码时，它就会调用这个Notify方法，用户程序可以从这个Nodify方法中得到通知（重写ICharsetDetectionObserver接口的Notify实现）。

# 代码实例：
```C#
//实现ICharsetDetectionObserver接口
    public class MyCharsetDetectionObserver :
        NChardet.ICharsetDetectionObserver
    {
        public string Charset = null;
        
        public void Notify(string charset)
        {
            Charset = charset;
        }
    }
        int lang = 2 ;//
    //用指定的语参数实例化Detector
        Detector det = new Detector(lang) ;
    //初始化
        MyCharsetDetectionObserver cdo = new MyCharsetDetectionObserver();
        det.Init(cdo);

    //输入字符流
    Uri url = new Uri(“http://cn.yahoo.com”);
    HttpWebRequest request =
        HttpWebRequest)WebRequest.Create(url);
    HttpWebResponse response =
        (HttpWebResponse)request.GetResponse();
    Stream stream = response.GetResponseStream();
    
    byte[] buf = new byte[1024] ;
    int len;
    bool done = false ;
    bool isAscii = true ;

    while( (len=stream.Read(buf,0,buf.Length)) != 0) {
        // 探测是否为Ascii编码
        if (isAscii)
            isAscii = det.isAscii(buf,len);

        // 如果不是Ascii编码，并且编码未确定，则继续探测
        if (!isAscii && !done)
                done = det.DoIt(buf,len, false);

    }
    stream.Close();
    stream.Dispose();
    //调用DatEnd方法，
    //如果引擎认为已经探测出了正确的编码，
    //则会在此时调用ICharsetDetectionObserver的Notify方法
    det.DataEnd();

    if (isAscii) {
        Console.WriteLine("CHARSET = ASCII");
          found = true ;
    }
    else if (cdo.Charset != null)
    {
        Console.WriteLine("CHARSET = {0}",cdo.Charset);
        found = true;
    }
    
    if (!found) {
        string[] prob = det.getProbableCharsets() ;
        for(int i=0; i<prob.Length; i++) {
            Console.WriteLine("Probable Charset = " + prob[i]);
        }
    }
    Console.ReadLine();
