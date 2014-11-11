using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers {

    public class BooksController : ApiController {

        public object Get() {
            return new {
                books = new[] {
                    new {id = 1, title = "Three Little Pigs", author = "Abby Goldstein"},
                    new {id = 2, title = "Little Red Riding Hood", author = "Michael Smith"},
                    new {id = 3, title = "Snail and the Whale", author = "Julia Donaldson"},
                    new {id = 4, title = "Under the Sea", author = "Wally Snowheart"},
                    new {id = 5, title = "The Big Mountain", author = "Simon Shoe"}
                }
            };
        }

    }

}
