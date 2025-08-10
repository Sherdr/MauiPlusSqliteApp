using MauiPlusSqliteApp.Data;
using MauiPlusSqliteApp.Models;

namespace MauiPlusSqliteApp.Services
{
    public class OperationsTodo
    {
        public static int Create(Todo todo)
        {
            int id = 0;
            using(var db = new TodoRepository())
            {
                db.Todolist.Add(todo);
                db.SaveChanges();
                id = todo.Id;
            }
            return id;
        }

        public static IEnumerable<Todo> Read()
        {
            List<Todo> list = new List<Todo>();
            using (var db = new TodoRepository())
            {
                list = db.Todolist.ToList();
            }
            return list;
        }

        public static Todo? Read(int id)
        {
            Todo? todo = null;
            using(var db = new TodoRepository())
            {
                todo = db.Find<Todo>(id); 
            }
            return todo;
        }

        public static void Update(Todo todo)
        {
            using(var db = new TodoRepository())
            {
                db.Todolist.Update(todo);
                db.SaveChanges();
            }
        }

        public static void Delete(Todo todo)
        {
            using(var db = new TodoRepository())
            {
                db.Todolist.Remove(todo);
                db.SaveChanges();
            }
        }
    }
}
