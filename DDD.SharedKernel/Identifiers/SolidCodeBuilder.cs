using System;
using System.Text;

namespace DDD.SharedKernel.Identifiers
{
    public class SolidCodeBuilder
    {
        private const char EntitySeparator = 'O';
        private readonly StringBuilder _sb;

        public SolidCodeBuilder()
        {
            _sb = new StringBuilder();
        }


        public void AppendNewSolidCode(Prefix prefix)
        {
            AddValueSeparatorIfNeeded();
            _sb.AppendPrefix(prefix);
            _sb.AppendNewSolidCode();
        }

        public void AppendGuid(Prefix prefix, Guid guid)
        {
            AddValueSeparatorIfNeeded();
            _sb.AppendPrefix(prefix);
            _sb.AppendAsSolidCode(guid);
        }

        public void AppendId(Prefix prefix, int id)
        {
            AddValueSeparatorIfNeeded();
            _sb.AppendPrefix(prefix);
            _sb.Append(id);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private void AddValueSeparatorIfNeeded()
        {
            if (_sb.Length > 0)
            {
                _sb.Append(EntitySeparator);
            }
        }

    }
}