/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2006-12-16
 * Time: 9:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;

namespace Test
{
	class MainClass
	{
		public static bool found = false;
		public static void Main(string[] args)
		{
            string[] files = System.IO.Directory.GetFiles(@"D:\Documents and Settings\桌面\新建文件夹");
            foreach (string filename in files)
            {
                System.Text.Encoding encoding = GetEncodingOfFile(filename);
                Console.WriteLine("{0} : {1}", filename, encoding.EncodingName);
                using (StreamReader reader = new StreamReader(filename, encoding,true))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            Console.Read();
        //    if (args.Length != 1 && args.Length != 2)
        //    {
        //        Console.WriteLine("Usage: Test.exe <url> [<languageHint>]");
        //        Console.WriteLine("");
        //        Console.WriteLine("Where <url> is http://...");
        //        Console.WriteLine("For optional <languageHint>. Use following...");
        //        Console.WriteLine("    1 => Japanese");
        //        Console.WriteLine("    2 => Chinese");
        //        Console.WriteLine("    3 => Simplified Chinese");
        //        Console.WriteLine("    4 => Traditional Chinese");
        //        Console.WriteLine("    5 => Korean");
        //        Console.WriteLine("    6 => Dont know (default)");
        //        Console.ReadLine();
        //        return;
        //    }
        //    int lang = (args.Length == 2)? int.Parse(args[1]) : PSMDetector.ALL ;
			
        //    Detector det = new Detector(lang) ;
        //    MyCharsetDetectionObserver cdo = new MyCharsetDetectionObserver();
        //    det.Init(cdo);
			
        //    Uri url = new Uri(args[0]);
        //    //Uri url = new Uri("http://cn.yahoo.com/");
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream stream = response.GetResponseStream();
			
        //    byte[] buf = new byte[1024] ;
        //    int len;
        //    bool done = false ;
        //    bool isAscii = true ;
			
        //    while( (len=stream.Read(buf,0,buf.Length)) != 0) {
        //        // Check if the stream is only ascii.
        //        if (isAscii)
        //            isAscii = det.isAscii(buf,len);

        //        // DoIt if non-ascii and not done yet.
        //        if (!isAscii && !done)
        //            done = det.DoIt(buf,len, false);
        //    }
			
        //    stream.Close();
			
        //    det.DataEnd();
			
        //    if (isAscii) {
        //        Console.WriteLine("CHARSET = ASCII");
        //        found = true ;
        //    }
        //    else if (cdo.Charset != null)
        //    {
        //        Console.WriteLine("CHARSET = {0}",cdo.Charset);
        //        found = true;
        //    }
			
        //    if (!found) {
        //        string[] prob = det.getProbableCharsets() ;
        //        for(int i=0; i<prob.Length; i++) {
        //            Console.WriteLine("Probable Charset = " + prob[i]);
        //        }
        //    }
        //    Console.ReadLine();
        }

        static System.Text.Encoding GetEncodingOfFile(string filename)
        {
            int count;
            byte[] buf;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                buf = new byte[fs.Length];
                count = fs.Read(buf, 0, buf.Length);
            }
            if (buf.Length > 2 && buf[0] == 0xEF && buf[1] == 0xBB && buf[2] == 0xBF)
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buf.Length >= 2 && buf[0] == 0xFF && buf[1] == 0xFE)
            {
                if (buf.Length >= 4 && buf[2] == 0x00 && buf[3] == 0x00)
                {
                    return System.Text.Encoding.UTF32;
                }
                else
                {
                    return System.Text.Encoding.Unicode;
                }
            }
            else if (buf.Length >= 2 && buf[0] == 0xFE && buf[1] == 0xFF)
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buf.Length >= 4 && buf[0] == 0x00 && buf[1] == 0x00 && buf[2] == 0xFE && buf[3] == 0xFF)
            {
                return System.Text.Encoding.GetEncoding("UTF-32BE");
            }
            else
            {
                MyCharsetDetectionObserver cdo = new MyCharsetDetectionObserver();
                NChardet.Detector detector = new NChardet.Detector();
                detector.Init(cdo);
                if (detector.isAscii(buf, count))
                {
                    return System.Text.Encoding.ASCII;
                }
                else
                {
                    detector.DoIt(buf, count, false);
                    detector.Done();
                    if (string.IsNullOrEmpty(cdo.Charset))
                    {
                        return System.Text.Encoding.Default;
                    }
                    else
                    {
                        return System.Text.Encoding.GetEncoding(cdo.Charset);
                    }
                }
            }
        }
	}	
}
