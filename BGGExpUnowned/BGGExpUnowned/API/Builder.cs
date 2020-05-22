using System;
using System.Collections.Generic;
using System.Text;

namespace com.mbpro.BGGExpUnowned.API
{
    public class Builder<T> where T : new()
    {
        private readonly IList<Action<T>> actions = new List<Action<T>>();
        public T Build()
        {
            var built = new T();
            foreach (var action in actions)
            {
                action(built);
            }
            return built;
        }
        public Builder<T> With(Action<T> with)
        {
            actions.Add(with);
            return this;
        }
    }
}
