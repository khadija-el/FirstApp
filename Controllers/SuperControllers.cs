using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FistApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Reflection;
using System.Linq.Expressions;

namespace FistApi.Controllers
{
    // //[Authorize]
    public class SuperController<T> : ControllerBase where T : class
    {
        protected readonly FistApiContext _context;

        public SuperController(FistApiContext context)
        {
            _context = context;
        }

        [HttpGet("{startIndex}/{pageSize}/{sortBy}/{sortDir}")]
        public virtual async Task<IActionResult> GetAll(int startIndex, int pageSize, string sortBy, string sortDir)
        {
            var list = await _context.Set<T>()
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync()
                ;
            int count = await _context.Set<T>().CountAsync();

            return Ok(new { list = list, count = count });
        }

        [HttpGet]
        public async Task<ActionResult<int>> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        [HttpGet("{column}/{name}")]
        public async Task<ActionResult<IEnumerable<T>>> Autocomplete([FromRoute] string column,[FromRoute] string name)
        {
            int i = typeof(T).FullName.LastIndexOf('.');
            string tableName = typeof(T).FullName.Substring(i + 1) + "s";

            return await _context.Set<T>()
                .FromSqlRaw(String.Format(@"SELECT * FROM {0} where {1} LIKE '%{2}%'", tableName, column, name.Replace("'", "''")))
                .Take(10)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<T>>> AutocompletePost(AutoCompleteObjet o)
        {
            int i = typeof(T).FullName.LastIndexOf('.');
            string tableName = typeof(T).FullName.Substring(i + 1) + "s";

            return await _context.Set<T>()
                .FromSqlRaw(String.Format(@"SELECT * FROM {0} where {1} LIKE '%{2}%'", tableName, o.Column, o.Name.Replace("'", "''")))
                .Take(10)
                .ToListAsync();
        }

        // public string methode(T e)
        // {
        //     PropertyInfo[] props = e.GetType().GetProperties();
        //     foreach (PropertyInfo prop in props)
        //     {
        //         if (prop.Name.ToString().ToLower() == "label" || prop.Name.ToString().ToLower() == "nom")
        //         {
        //             return prop.GetValue(e).ToString().ToLower();
        //         }
        //     }
        //     // int id = int.Parse(props[0].GetValue(e).ToString());

        //     return null;
        // }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return await _context.Set<T>().ToListAsync();
        }


        [HttpPost]
        public virtual async Task<ActionResult<T>> Post(T model)
        {
            _context.Set<T>().Add(model);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Created("", model);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Set<T>().Remove(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Accepted();
        }


        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] T model)
        {

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        [HttpPost]
        public virtual async Task<IActionResult> UpdateRange(List<T> models)
        {
            // Console.WriteLine("-----------------------------------------------------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(models));
            // Console.WriteLine("---------------------------------------------------------------- - ");

            if (models.Count == 0)
            {
                return Ok(new { message = "count = 0" });
            }

            _context.Set<T>().UpdateRange(models);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        public virtual async Task<IActionResult> PostRange(List<T> models)
        {
            // Console.WriteLine("-----------------------------------------------------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(models));
            // Console.WriteLine("---------------------------------------------------------------- - ");

            if (models.Count == 0)
            {
                return Ok(new { message = "count = 0" });
            }

            await _context.Set<T>().AddRangeAsync(models);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Created("", "ok");
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> Get(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteRange(List<int> models)
        {
            if (models.Count == 0)
            {
                return Ok(new { message = "count = 0" });
            }

            // generic List with no parameters
            // Type openType = typeof(List<>);

            // // To create a List<string>
            // Type[] tArgs = { typeof(int) };
            // Type target = openType.MakeGenericType(tArgs);

            // Create an instance - Activator.CreateInstance will call the default constructor.
            // This is equivalent to calling new List<string>().
            // List<int> result = (List<int>)Activator.CreateInstance(target);

            // models.ForEach(model => {
            //     PropertyInfo prop = model.GetType().GetProperty("Id");

            //     int id = (int)prop.GetValue(model, null);

            //     _context.Set<T>().Remove(_context.Set<T>().Find(id));

            // });

            // var l =  models.Select(model => (int)model.GetType().GetProperty("Id").GetValue(model, null)).ToList();
            var l =  models.Select(id => _context.Set<T>().Find(id)).ToList();

            _context.Set<T>().RemoveRange(l);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }


    }

    public class AutoCompleteObjet {
        public string Column { get; set; }
        public string Name { get; set; }
    }
}