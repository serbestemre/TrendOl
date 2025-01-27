﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrendOl.Common;
using TrendOl.DataAccessLayer.Abstract;
using TrendOl.Entities;

namespace TrendOl.DataAccessLayer.EntityFramework
{
	public class Repository<T> :RepositoryBase , IRepository<T> where T: class
	{
		private DbSet<T> _objectSet;

		public Repository()
		{
			_objectSet = context.Set<T>();
		}
		public List<T> List()
		{
			return _objectSet.ToList();
		}

		public IQueryable<T> ListQueryable()
		{
			return _objectSet.AsQueryable<T>();
		}

		public List<T> List(Expression<Func<T,bool>> where)
		{
			return _objectSet.Where(where).ToList();
		}

		public int Insert(T obj)
		{
			_objectSet.Add(obj);

			if(obj is MyEntityBase)
			{
				MyEntityBase o = obj as MyEntityBase;
				DateTime now = DateTime.Now;

				o.CreatedOn = now;
				o.ModifiedOn = now;
				o.ModifiedUsername = App.Common.GetCurrentUsername();    // TODO there must come username who insert into the db
			}

			return Save();
		}

		public int Update(T obj)
		{


			if (obj is MyEntityBase)
			{
				MyEntityBase o = obj as MyEntityBase;
				o.ModifiedOn = DateTime.Now;
				o.ModifiedUsername = App.Common.GetCurrentUsername();  // TODO there must come username who update the tuple on the db
			}

			return Save();
		}

		public int Delete(T obj)
		{
			_objectSet.Remove(obj);
			return Save();
		}

		public T Find(Expression<Func<T,bool>> where)
		{
			return _objectSet.FirstOrDefault(where);
		}

		public int Save()
		{
			return context.SaveChanges();
		}
	}
}
