﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class UserCollection : Collection<User>
    {
        public UserCollection(GitLabObject container)
            : base(container)
        {
        }

        public User GetById(int id) => this.FirstOrDefault(user => user.Id == id);

        public override void Add(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (user.Id == default)
            {
                user.Id = GetNewId();
            }
            else if (GetById(user.Id) != null)
            {
                throw new GitLabException("User already exists");
            }

            base.Add(user);
        }

        private int GetNewId()
        {
            return this.Select(user => user.Id).DefaultIfEmpty().Max() + 1;
        }

        internal IEnumerable<User> SearchByUsername(string username)
        {
            return this.Where(user => string.Equals(user.UserName, username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
