using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class CharsetDetectionObserver : NChardet.ICharsetDetectionObserver
    {
        public string Charset = null;

        public void Notify(string charset)
        {
            Charset = charset;
        }
    }

    class EncodeDetector
    {
        public System.Text.Encoding GetEncodingOfFile(string filename)
        {
            int count = 0;
            byte[] buf;
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                buf = new byte[fs.Length];
                count = fs.Read(buf, 0, buf.Length);
            }
            if (count < 1) return System.Text.Encoding.Default;
            NChardet.Detector detect = new NChardet.Detector();
            CharsetDetectionObserver cdo = new CharsetDetectionObserver();
            detect.Init(cdo);
            if (detect.isAscii(buf,count))
            {
                return System.Text.Encoding.ASCII;
            }
            else
            {
                detect.DoIt(buf, count, true);
                detect.DataEnd();
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
