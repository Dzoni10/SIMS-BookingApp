using BookingApp.Domain.Models;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using System.Windows;


namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for CommentsOverview.xaml
    /// </summary>
    public partial class CommentsOverview : Window
    {

        public static ObservableCollection<Comment> Comments { get; set; }

        public Comment SelectedComment { get; set; }

        public User LoggedInUser { get; set; }

        private CommentRepository commentRepository;

        public CommentsOverview(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            commentRepository = new CommentRepository();
            Comments = new ObservableCollection<Comment>(commentRepository.FindCommentByUser(user));
        }

        private void ShowCreateCommentForm(object sender, RoutedEventArgs e)
        {
            CommentForm createCommentForm = new CommentForm(LoggedInUser);
            createCommentForm.Show();
        }

        private void ShowViewCommentForm(object sender, RoutedEventArgs e)
        {
            if (SelectedComment != null)
            {
                CommentForm viewCommentForm = new CommentForm(SelectedComment);
                viewCommentForm.Show();
            }
        }

        private void ShowUpdateCommentForm(object sender, RoutedEventArgs e)
        {
            if (SelectedComment != null)
            {
                CommentForm updateCommentForm = new CommentForm(SelectedComment, LoggedInUser);
                updateCommentForm.Show();
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (SelectedComment != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Delete comment",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    commentRepository.Delete(SelectedComment);
                    Comments.Remove(SelectedComment);
                }
            }
        }
    }
}
