using Com.Scm.Terminal;

namespace Com.Scm
{
    public interface ITerminalHolder
    {
        public ScmTerminalInfo GetTerminal(long id);

        public void Remote(long id);

        public void Clear();
    }
}
