using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Posts Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{post.Title} ({post.Url})");
            }
        }
        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title} ({post.Url})");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        private Author ChooseAuthor(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Author> authors = _authorRepository.GetAll();

            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($" {i + 1}) {author.FullName}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return authors[choice - 1];
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid Selection. Cannot add/update author.");
                }
                return null;
            }
        }
        private Blog ChooseBlog(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a blog:";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.ToString()}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid Selection. Cannot add/update blog.");
                }
                return null;
            }
        }
        private void Add()

        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            bool success = false;
            DateTime publishDate;
            do
            {
                Console.Write("Publish Date (e.g. 1/1/1990): ");
                success = DateTime.TryParse(Console.ReadLine(), out publishDate);
            }
            while (!success);
            post.PublishDateTime = publishDate;
            
            Author chosenAuthor = ChooseAuthor("Which person is the author of the post?");
            while (chosenAuthor == null)
            {
                chosenAuthor = ChooseAuthor("Please choose an author.");
            }
            post.Author = chosenAuthor;

            Blog chosenBlog = ChooseBlog("Which blog does this post belong to?");
            while (chosenBlog == null)
            {
                chosenBlog = ChooseBlog("Please choose a blog.");
            }
            post.Blog = chosenBlog;



            _postRepository.Insert(post);



        }
        private void Edit()
        {
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }

            Console.Write("New Url (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }
            

            bool success = false;
            DateTime publishDate;
            string publishDateStr = "";
            do
            {
                Console.Write("New Publish Date (e.g. 1/1/2022) (blank to leave unchanged): ");
                publishDateStr = Console.ReadLine();
                success = DateTime.TryParse(publishDateStr, out publishDate);
            }
            while (!success && !string.IsNullOrWhiteSpace(publishDateStr));
            if(!string.IsNullOrWhiteSpace(publishDateStr))
            {
                postToEdit.PublishDateTime = publishDate;
            }

            Author chosenAuthor = ChooseAuthor("Choose new author (blank to leave unchanged): ");
            if (chosenAuthor != null)
            {
                postToEdit.Author = chosenAuthor;
            }

            Blog chosenBlog = ChooseBlog("Which blog does this post belong to? (blank to leave unchanged): ");
            if (chosenBlog != null)
            {
                postToEdit.Blog = chosenBlog;
            }
            postToEdit.Blog = chosenBlog;

            _postRepository.Update(postToEdit);
        }
        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            _postRepository.Delete(postToDelete.Id);
        }
    }
}
