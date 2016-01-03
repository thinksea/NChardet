/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2007-1-27
 * Time: 22:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NChardet;

namespace Test
{
	/// <summary>
	/// Description of MyCharsetDetectionObserver.
	/// </summary>
	public class MyCharsetDetectionObserver : NChardet.ICharsetDetectionObserver
	{
		public string Charset = null;
		
		public void Notify(string charset)
		{
			Charset = charset;
		}
	}
}
