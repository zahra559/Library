using LiabraryApp.Models;
using LiabraryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;

namespace LiabraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IGenericRepository<CUser> _userRepository;
        private readonly IGenericRepository<CBorrowing> _borrowingRepository;

        public HomeController(ILogger<HomeController> logger,
            IBookRepository bookRepository,
            IGenericRepository<CUser> userRepository,
            IGenericRepository<CBorrowing> borrowingRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _borrowingRepository = borrowingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetBooks([FromBody] CBook book)
        {
            try
            {
                Hashtable criteria = new Hashtable();
                if (book.Name != string.Empty)
                    criteria.Add("NAME", book.Name);

                if (book.Author != string.Empty)
                    criteria.Add("AUTHOR", book.Author);

                if (book.ISBN != string.Empty)
                    criteria.Add("ISBN", book.ISBN);

                List<CBook> _books = await _bookRepository.Get(criteria);
                return Ok(_books);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> GetBrrowings([FromBody] CBorrowing borrowing)
        {
            try
            {
                Hashtable criteria = new Hashtable();
                if (borrowing.Book.ID != 0)
                    criteria.Add("BOOK_ID", borrowing.Book.ID);

                if (borrowing.User.ID != 0)
                    criteria.Add("USER_ID", borrowing.User.ID);

                if (borrowing.Book.ISBN != string.Empty)
                    criteria.Add("BOOK_ISBN", borrowing.Book.ISBN);

                List<CBorrowing> _borrowings = await _borrowingRepository.Get(criteria);
                return Ok(_borrowings);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetUsers([FromBody] CUser user)
        {
            try
            {
                Hashtable criteria = new Hashtable();
                if (user.ID != 0)
                    criteria.Add("ID", user.ID);

                if (user.Name != string.Empty)
                    criteria.Add("NAME", user.Name);

                List<CUser> _users = await _userRepository.Get(criteria);
                return Ok(_users);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> ReturningBook([FromBody] CBorrowing borrowing)
        {
            try
            {
                Hashtable criteria = new Hashtable();
                criteria.Add("NAME", borrowing.User.Name);
                List<CUser> _users = await _userRepository.Get(criteria);

                if(_users.Count > 0)
                {
                    criteria = new Hashtable();
                    criteria.Add("USER_ID", _users[0].ID);
                    criteria.Add("BOOK_ID", borrowing.Book.ID);

                    List<CBorrowing> _borrowings = await _borrowingRepository.Get(criteria);
                    if (_borrowings.Count > 0)
                    {
                        bool returned = await _bookRepository.ReturningBook(criteria);
                    }
                    else
                    {
                        return BadRequest("This book is not borrowed");
                    }
                        return Ok();
                }
                else
                {
                    return BadRequest("The user is not in the system");
                }

            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<IActionResult> BorrowingBook([FromBody] CBorrowing borrowing)
        {
            try
            {
                Hashtable criteria = new Hashtable();
                criteria.Add("NAME", borrowing.User.Name);
                List<CUser> _users = await _userRepository.Get(criteria);

                if (_users.Count > 0)
                {
                    criteria = new Hashtable();
                    criteria.Add("USER_ID", _users[0].ID);
                    criteria.Add("BOOK_ID", borrowing.Book.ID);

                    List<CBorrowing> _borrowings = await _borrowingRepository.Get(criteria);
                    if (_borrowings.Count > 0)
                    {
                        return BadRequest("This book is borrowed");
                    }
                    else
                    {
                        bool returned = await _bookRepository.BorrowingBook(criteria);
                    }
                    criteria = new Hashtable();
                    criteria.Add("ID", borrowing.Book.ID);
                    List<CBook> books = await _bookRepository.Get(criteria);
                    return Ok(books[0]);
                }
                else
                {
                    return BadRequest("The user is not in the system");
                }

            }
            catch
            {
                return BadRequest();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}