using System;
using System.IO;
using System.Linq;

namespace AutofacLab
{
	public class Memo
	{
		public string Title;
		public DateTime DueAt;
	}

	public interface IMemoDueNotifier
	{
		void MemoIsDue(Memo memo);
	}

	public class MsgBoxNotifier : IMemoDueNotifier 
	{
		public void MemoIsDue(Memo memo) 
		{
			System.Windows.Forms.MessageBox.Show(
				string.Format("Memo '{0}' is due!", memo.Title)
			);
		}
	}


	public class PrintingNotifier : IMemoDueNotifier
	{
		TextWriter _writer;

		public PrintingNotifier(TextWriter writer) 
		{
			_writer = writer;
		}

		public void MemoIsDue(Memo memo)
		{
			_writer.WriteLine("Memo '{0}' is due!", memo.Title);
		}
	}

	public class MemoChecker 
	{
		IQueryable<Memo> _memos;
		IMemoDueNotifier _notifier;

		public MemoChecker(IQueryable<Memo> memos, IMemoDueNotifier notifier) 
		{
			_memos = memos;
			_notifier = notifier;
		}

		public void CheckNow() 
		{
			var overdueMemos = _memos.Where(memo => memo.DueAt < DateTime.Now);

			foreach (var memo in overdueMemos)
				_notifier.MemoIsDue(memo);
		}


	}

}
