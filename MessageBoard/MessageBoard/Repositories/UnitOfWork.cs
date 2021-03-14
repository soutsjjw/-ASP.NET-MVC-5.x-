using MessageBoard.Data;
using MessageBoard.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Text;

namespace MessageBoard.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MessageBoardContext _context;
        private bool _disposed = false;

        #region Constructor

        /// <summary>
        /// 設定此Unit of work(UOF)的Context。
        /// </summary>
        /// <param name="context">設定UOF的context</param>
        public UnitOfWork(MessageBoardContext context)
        {
            _context = context;
        }

        #endregion Constructor

        public void Commit()
        {
            ILogger _log = LogManager.GetCurrentClassLogger();

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"DbUpdateException error details - {ex?.InnerException?.InnerException?.Message}");

                foreach (var e in ex.Entries)
                {
                    sb.AppendLine($"Entity of type {e.Entity.GetType().Name} in state {e.State} could not be updated");
                }

                _log.Error(sb.ToString());

                throw;
            }
            catch (Exception ex)
            {
                _log.Error("Catch else 1::\n" + ex.ToString());
                _log.Error("Catch else 2::\n" + ex.InnerException.ToString());

                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }
            }

            _disposed = true;
        }
    }
}
