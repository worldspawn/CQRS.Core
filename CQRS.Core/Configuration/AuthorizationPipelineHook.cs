using System;
using EventStore;

namespace CQRS.Core.Configuration
{
    public class AuthorizationPipelineHook : IPipelineHook
    {
        #region IPipelineHook Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Commit Select(Commit committed)
        {
            // return null if the user isn't authorized to see this commit
            return committed;
        }

        public virtual bool PreCommit(Commit attempt)
        {
            // Can easily do logging or other such activities here
            return true; // true == allow commit to continue, false = stop.
        }

        public virtual void PostCommit(Commit committed)
        {
            // anything to do after the commit has been persisted.
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            // no op
        }
    }
}