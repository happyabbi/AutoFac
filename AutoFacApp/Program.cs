using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using AutofacLab;

namespace AutoFacApp
{
	class MainClass
	{
		static IQueryable<Memo> GenSomeMemos()
		{ 
			IQueryable<Memo> memos = new List<Memo>() {
				new Memo { Title = "Release Autofac 1.0",
						   DueAt = new DateTime(2007, 12, 14) },
				new Memo { Title = "Write CodeProject Article",
						   DueAt = DateTime.Now },
				new Memo { Title = "End of The World",
						   DueAt = new DateTime(2012, 12, 21) }
			}.AsQueryable();
			return memos;		
		}

		static void NoDI()
		{
			//傳統寫法，物件的產生是寫死的
			var chkr = new MemoChecker(GenSomeMemos(),
				new PrintingNotifier(Console.Out));
			chkr.CheckNow();
		}

		static ContainerBuilder builder;
		public static void Main(string[] args)
		{
			builder = new ContainerBuilder();
			builder.Register(c => new MemoChecker(
				c.Resolve<IQueryable<Memo>>(),
				c.Resolve<IMemoDueNotifier>()));

			//builder.Register(c => new PrintingNotifier(
			//	c.Resolve<TextWriter>())).As<IMemoDueNotifier>();

			builder.Register(c => new MsgBoxNotifier()).AsImplementedInterfaces();

			IQueryable<Memo> memos = GenSomeMemos();

			builder.RegisterInstance(memos);

			builder.RegisterInstance(Console.Out).As<TextWriter>().ExternallyOwned();

			//builder.Register<TextWriter>(c => new StreamWriter(
			//	string.Format("F:\\{0:yyyyMMdd}.log", DateTime.Today), true
			//));

			InAutofacWay();

			Console.WriteLine("Done! press any key to exit...");
			Console.Read();
		}

		static void InAutofacWay() 
		{
			//使用using包住Container
			//當程式結束時Autofac會自動處理需要Dispose的物件
			using (var container = builder.Build()) 
			{
				container.Resolve<MemoChecker>().CheckNow();
			}
		}
	}
}
