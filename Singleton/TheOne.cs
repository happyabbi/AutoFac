using System;
using System.Threading;

namespace Singleton
{
	public class TheOne
	{		
		private Guid UniqueKey = Guid.NewGuid();

		private static TheOne instance = null;
		private static object syncRoot = new Object();

		public static TheOne Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new TheOne();
					}
				}
				return instance;
			}
		}

		/// <summary>
		/// 建構式
		/// </summary>
		private TheOne()
		{
			Thread.Sleep(2000);
			Console.WriteLine("Constructor Executed");
		}

		public void ShowUniqueKey()
		{
			Console.WriteLine("Unique Key={0}", UniqueKey);
		}
	}
}
