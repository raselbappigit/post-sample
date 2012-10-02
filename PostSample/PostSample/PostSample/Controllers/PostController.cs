using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PostSample.Models;
using PostSample.Helpers;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Data;

namespace PostSample.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/

        PostContext db = new PostContext();

        public ActionResult Index()
        {
            var _posts = db.Posts.ToList().OrderByDescending(x => x.PostId);


            var pcom = _posts.FirstOrDefault().PostComments.ToList();
            string str1 = "";
            string content1 = "";
            string content2 = "";
            string content3 = "";
            string content4 = "";
            string strConFin = "";
            string comRpl;
            PostComment PComPrev = new PostComment();
            PostComment PComNext = new PostComment();
            bool IsLast = false;
            foreach (var post in _posts.ToList())
            {

                foreach (var itemComment in post.PostComments.ToList())
                {

                    foreach (var itemInn in itemComment.PostCommentChilds.ToList())
                    {
                        //Comment Level 2 <Post Comment Reply>
                        var pCom = itemInn;

                        for (; pCom.PostCommentChilds.Count() > 0; )
                        {
                            //pCom = pCom;
                            if (!IsLast)
                            {
                                content1 = pCom.Content;
                                strConFin = content1;
                            }
                            else
                            {
                                pCom = pCom.PostCommentChilds.FirstOrDefault();
                                content1 = pCom.Content;
                                strConFin = content1;
                                break;
                            }


                            str1 = "";
                            foreach (var itemRepInn in pCom.PostCommentChilds)
                            {
                                PComPrev = pCom;

                                if (itemRepInn.PostCommentChilds.Count() > 0)
                                {
                                    pCom = itemRepInn;
                                }
                                else
                                {
                                    IsLast = true;
                                    strConFin = itemRepInn.Content;

                                }
                                //pCom = itemRepInn;


                                PComNext = pCom;
                                str1 = "";
                                comRpl = pCom.Content;
                            }
                            content3 = PComPrev.Content;
                            content4 = PComNext.Content;


                        }
                        comRpl = pCom.Content;
                        str1 = "";


                    }
                }

            }



            ///////////////////////////////////////////////////////////
            ////////PostComment pComReply = new PostComment();

            ////////string strConFin = "";
            ////////string stComment = "";
            ////////string str1 = "";
            ////////string tt = "";

            ////////foreach (var post in _posts.ToList())
            ////////{

            ////////    foreach (var itemComment in post.PostComments.ToList())
            ////////    {
            ////////        //Comment Level 1 <Post Comment>
            ////////        tt = itemComment.Content;
            ////////        str1 = "";

            ////////        foreach (var itemInn in itemComment.PostCommentChilds.ToList())
            ////////        {
            ////////            //Comment Level 2 <Post Comment Reply>
            ////////            var pCom = itemInn;
            ////////            bool IsLast = false;
            ////////            for (; pCom.PostCommentChilds.Count() > 0 || pCom.PostCommentChilds.Count() == 0; )
            ////////            {
            ////////                if (!IsLast)
            ////////                {
            ////////                    pComReply = pCom;

            ////////                }
            ////////                else
            ////////                {
            ////////                    pCom = pCom.PostCommentChilds.FirstOrDefault();
            ////////                    pComReply = pCom;

            ////////                }
            ////////                //Comment Reply Display Level
            ////////                stComment = pComReply.Content;
            ////////                str1 = "";

            ////////                if (pCom.PostCommentChilds.Count() == 0)
            ////////                {
            ////////                    IsLast = true;
            ////////                }


            ////////                if (IsLast)
            ////////                {
            ////////                    break;
            ////////                }

            ////////                foreach (var itemRepInn in pCom.PostCommentChilds)
            ////////                {
            ////////                    if (itemRepInn.PostCommentChilds.Count() > 0)
            ////////                    {
            ////////                        pCom = itemRepInn;
            ////////                    }
            ////////                    else
            ////////                    {
            ////////                        IsLast = true;
            ////////                    }
            ////////                }

            ////////            }
            ////////            str1 = "";

            ////////        }
            ////////    }

            ////////}
            /////////////////////////////////////////////////////////////////////////////


            return View(_posts);
        }

        //for post save
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(string content)
        {
            string message = string.Empty;

            Post _post = new Post();

            string _loginUser = User.Identity.Name.ToString();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                string _postContent = content;

                _post.Content = _postContent;
                //_post.CreatedBy = "rasel";
                _post.CreatedDate = DateTime.Now;
                _post.PostTypeId = 2;//post type 

                _post.User = _CurrUser;

                try
                {
                    db.Posts.Add(_post);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_Post", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //for post wise comments

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddComment(int id, string content)
        {
            string message = string.Empty;

            PostComment _postComment = new PostComment();

            string _loginUser = User.Identity.Name.ToString();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                int _postId = id;
                string _postCommentContent = content;

                Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

                if (_post != null)
                {
                    _post.TotalComments += 1;

                    _postComment.Post = _post;
                    _postComment.PostId = _postId;
                    _postComment.Content = _postCommentContent;
                    //_postComment.CreatedBy = _loginUser;
                    _postComment.CreatedDate = DateTime.Now;
                    //_postComment.PerantFlagId = 1;

                    _postComment.User = _CurrUser;
                }

                try
                {
                    //post counter ++
                    db.Posts.Attach(_post);
                    db.Entry(_post).State = EntityState.Modified;

                    db.PostComments.Add(_postComment);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostComment", _postComment),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddCommentToComment(int postId, int comId, string content)
        {
            string message = string.Empty;

            PostComment _postComment = new PostComment();
            PostComment postComChild = new PostComment();

            //PostComment _postCommentView = new PostComment();

            string _loginUser = User.Identity.Name.ToString();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                int _postId = postId;
                int _commentId = comId;
                string _postCommentContent = content;

                _postComment = db.PostComments.Where(x => x.CommentId == _commentId).FirstOrDefault();

                Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

                postComChild.Content = _postCommentContent;
                postComChild.CreatedDate = DateTime.Now;
                //pComChild.Child = _postComment;
                //pComChild.PostCommentChilds = _postComment;
                //pComChild.Post = _post;
                //pComChild.PostId = _post.PostId;

                //_postComment.PostId = _postId;
                //_postComment.Content = _postCommentContent;
                ////_postComment.CreatedBy = _loginUser;
                //_postComment.CreatedDate = DateTime.Now;

                postComChild.User = _CurrUser;

                //List<PostComment> lstPComChild = new List<PostComment>();
                //lstPComChild.Add(pComChild);

                //_tempCom.PostCommentChilds = lstPComChild;
                //PostCommentChilds
                //pComChild.PostCommentChilds = _postComment;

                postComChild.PerantPostComment = _postComment;

                //if (_postComment.PostComments==null)
                //{
                //    _postComment.PostComments = new List<PostComment>() {pComChild };
                //}
                //else
                //{
                //    _postComment.PostComments.Add(pComChild);
                //}

                //_postCommentView.CommentId = _postComment.CommentId;
                //_postCommentView.Content = _postComment.Content;
                //_postCommentView.CreatedDate = _postComment.CreatedDate;
                //_postCommentView.PostId = postComChild.PostId;
                ////_postCommentView.PerantFlagId = _postComment.PerantFlagId;
                //_postCommentView.User = _postComment.User;
                //_postCommentView.PostCommentChilds = lstPComChild;


                try
                {
                    //db.PostComments.Attach(_tempCom);
                    //db.Entry(_tempCom).State = EntityState.Modified;

                    //db.PostComments.Add(_postComment);

                    //db.PostComments.Add(pComChild);

                    //db.PostComments.Attach(_postComment);
                    //db.Entry(_postComment).State = EntityState.Modified;

                    _post.TotalComments += 1;

                    //post counter ++
                    db.Posts.Attach(_post);
                    db.Entry(_post).State = EntityState.Modified;

                    _postComment.PostCommentChilds.Add(postComChild);

                    db.SaveChanges();

                    //do  not insert just for display
                    postComChild.PostId = _post.PostId;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostCommentReplay", postComChild),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //PostUpdateForm
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostUpdateForm(int id)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            if (id == 1)
            {
                return PartialView("_PostUpdateForm");
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostQuestionForm
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostQuestionForm(int id)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            if (id == 2)
            {
                return PartialView("_PostQuestionForm");
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostEventForm
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostEventForm(int id)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            if (id == 3)
            {
                return PartialView("_PostEventForm");
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostPollForm
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostPollForm(int id)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            if (id == 4)
            {
                return PartialView("_PostPollForm");
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostUpdate
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostUpdate(string content)
        {
            string message = string.Empty;

            Post _post = new Post();

            string _loginUser = User.Identity.Name.ToString();

            //User _user = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                string _postContent = content;

                _post.Content = _postContent;
                //_post.CreatedBy = "rasel";
                _post.CreatedDate = DateTime.Now;
                _post.PostTypeId = 1; //post type for update type id

                _post.User = _CurrUser;

                try
                {
                    //one to one relational table data insert
                    db.Posts.Add(_post);
                    db.SaveChanges();

                    //_post.UpdatePost = new UpdatePost { PostId = _post.PostId };
                    //db.SaveChanges();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostUpdate", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

            //return PartialView("_Post", _post);
        }

        //PostQuestion
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostQuestion(string content)
        {
            string message = string.Empty;

            Post _post = new Post();

            string _loginUser = User.Identity.Name.ToString();

            //User _user = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                string _postContent = content;

                _post.Content = _postContent;
                //_post.CreatedBy = "rasel";
                _post.CreatedDate = DateTime.Now;
                _post.PostTypeId = 2; //post type for question type id

                _post.User = _CurrUser;

                try
                {
                    //one to one relational table data insert
                    db.Posts.Add(_post);
                    db.SaveChanges();

                    //_post.QuestionPost = new QuestionPost { PostId = _post.PostId };
                    //db.SaveChanges();

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostQuestion", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

            //return PartialView("_Post", _post);
        }

        //PostEvent
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostEvent(string content, string month, string date, string time, string duration, string venue, string note)
        {
            string message = string.Empty;

            //string content = postUpdateBox;

            Post _post = new Post();

            string _loginUser = User.Identity.Name.ToString();

            //User _user = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                string _postContent = content;
                int _postEventMonth = Convert.ToInt32(month);
                int _postEventDate = Convert.ToInt32(date);
                string _postEventTime = time;
                string _postEventDuration = duration;
                string _postEventVenue = venue;
                string _postEventNote = note;

                DateTime _eventTime = DateTime.Parse(_postEventTime);

                DateTime _eventDateTime = new DateTime(DateTime.Now.Year, _postEventMonth, _postEventDate, _eventTime.Hour, _eventTime.Minute, _eventTime.Millisecond);

                _post.Content = _postContent;
                //_post.CreatedBy = "rasel";
                _post.CreatedDate = DateTime.Now;
                _post.PostTypeId = 3; // post type for event type id

                _post.User = _CurrUser;

                try
                {
                    //one to one relational table data insert
                    db.Posts.Add(_post);
                    db.SaveChanges();

                    _post.EventPost = new EventPost { PostId = _post.PostId, EventDateTime = _eventDateTime, EventDuration = _postEventDuration, EventVenue = _postEventVenue, EventNote = _postEventNote };
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostEvent", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

            //return PartialView("_Post", _post);
        }

        //PostPoll
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostPoll(string content, string answers)
        {
            string message = string.Empty;

            Post _post = new Post();

            string _loginUser = User.Identity.Name.ToString();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            //for item list
            List<string> itemAnswer = serializer.Deserialize<List<string>>(answers);

            //User _user = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(content))
            {
                string _postContent = content;

                _post.Content = _postContent;
                //_post.CreatedBy = "rasel";
                _post.CreatedDate = DateTime.Now;
                _post.PostTypeId = 4; //post type for poll type id

                _post.User = _CurrUser;

                try
                {
                    //one to one relational data insert
                    db.Posts.Add(_post);
                    db.SaveChanges();

                    _post.PollPost = new PollPost { PostId = _post.PostId };
                    db.SaveChanges();

                    PollPost _tempPoll = db.PollPosts.Where(x => x.PostId == _post.PostId).FirstOrDefault();

                    if (_tempPoll != null)
                    {
                        int _pAnsId = db.PollAnswers.Count();

                        foreach (var item in itemAnswer.ToList())
                        {
                            _pAnsId++;
                            PollAnswers _pollAnswer = new PollAnswers();
                            _pollAnswer.Id = "" + _pAnsId;
                            _pollAnswer.AnswerContent = item;
                            _pollAnswer.PollPostId = _tempPoll.Id;
                            db.PollAnswers.Add(_pollAnswer);
                        }
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "error!";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostPoll", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

            //return PartialView("_Post", _post);
        }

        //PostPoll Vote 
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostPollVote(string select, string pollId, string id)
        {
            string message = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int postId = Convert.ToInt32(id);
            int postPollId = Convert.ToInt32(pollId);
            string postPollAnswerId = select;

            Post _tempPost = db.Posts.Where(x => x.PostId == postId).FirstOrDefault();

            PollPost _tempPollPost = db.PollPosts.Where(x => x.Id == postPollId).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(select) && _tempPost != null && _CurrUser != null)
            {

                try
                {
                    if (_tempPollPost != null)
                    {

                        bool isFound = false;

                        User prePollUser;

                        if (_tempPollPost.PollAnswers.Count() > 0)
                        {
                            var vPollAns = _tempPollPost.PollAnswers.ToList();

                            var vPollUsrs = from ans in vPollAns
                                            from usr in ans.Users.ToList()
                                            select usr;

                            if (vPollUsrs.Count() > 0)
                            {

                                var tempPollUsr = from tempans in vPollAns
                                                  from tempusr in tempans.Users.Where(x => x.UserName == _loginUser)
                                                  select tempusr;

                                prePollUser = tempPollUsr.FirstOrDefault();

                                if (tempPollUsr.Count() > 0)
                                {
                                    isFound = true;

                                    List<User> lstUser = new List<User>();
                                    lstUser.Add(_CurrUser);

                                    var _tempPollAnsForRemove = from tempFrR in vPollAns
                                                                from tempPollAnsFrRemove in tempFrR.Users.Where(x => x.UserName == _loginUser)
                                                                select tempFrR;

                                    PollAnswers _tempPollAnsRemove = _tempPollAnsForRemove.FirstOrDefault();
                                    _tempPollAnsRemove.AnswerCount -= 1;
                                    _tempPollAnsRemove.Users.Remove(_CurrUser);

                                    db.SaveChanges();

                                    var chkPollAns = from tempForTest in vPollAns
                                                     from tempPollAnsForTest in tempForTest.Users.Where(x => x.UserName == _loginUser)
                                                     select tempForTest;

                                    if (chkPollAns.Count() == 0)
                                    {
                                        PollAnswers _tempPollAnswer = _tempPollPost.PollAnswers.Where(x => x.Id == postPollAnswerId).FirstOrDefault();
                                        _tempPollAnswer.AnswerCount += 1;
                                        _tempPollAnswer.Users = lstUser;

                                        db.PollAnswers.Attach(_tempPollAnswer);
                                        db.Entry(_tempPollAnswer).State = EntityState.Modified;

                                        db.SaveChanges();
                                    }


                                }

                            }
                        }

                        if (isFound == false)
                        {
                            List<User> lstUser = new List<User>();
                            lstUser.Add(_CurrUser);

                            PollAnswers _tempPollAnswer = _tempPollPost.PollAnswers.Where(x => x.Id == postPollAnswerId).FirstOrDefault();
                            _tempPollAnswer.AnswerCount += 1;
                            _tempPollAnswer.Users = lstUser;

                            db.PollAnswers.Attach(_tempPollAnswer);
                            db.Entry(_tempPollAnswer).State = EntityState.Modified;

                            _tempPollPost.TotalVote += 1;

                            db.PollPosts.Attach(_tempPollPost);
                            db.Entry(_tempPollPost).State = EntityState.Modified;

                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        message = "Do not insert";
                    }

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "Do not insert";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PollVoteResult", _tempPollPost),
                Message = message
            }, JsonRequestBehavior.AllowGet);

            //return PartialView("_Post", _post);
        }

        //PostUpdateForm
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostPollVoteOption(string id, string pollId)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            int postId = Convert.ToInt32(id);
            int postPollId = Convert.ToInt32(pollId);

            Post _post = db.Posts.Where(x => x.PostId == postId && x.PollPostId == postPollId).FirstOrDefault();

            PollPost _postPoll = _post.PollPost;

            if (_postPoll != null)
            {
                return PartialView("_PollVoteOption", _postPoll);
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostPollVoteReload
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PostPollVoteReload(string id, string pollId)
        {
            //log in user
            string _loginUser = User.Identity.Name.ToString();

            int postId = Convert.ToInt32(id);
            int postPollId = Convert.ToInt32(pollId);

            Post _post = db.Posts.Where(x => x.PostId == postId && x.PollPostId == postPollId).FirstOrDefault();

            PollPost _tempPollPost = _post.PollPost;

            if (_post != null)
            {
                return PartialView("_PollVoteResult", _tempPollPost);
            }
            else
            {
                return Content(Boolean.FalseString);
            }

        }

        //PostLikeSubmit
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostLikeSubmit(string id)
        {
            string message = string.Empty;
            string html = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = Convert.ToInt32(id);

            Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(id) && _post != null)
            {

                try
                {
                    bool isLike = false;

                    if (_post.LikeStatues.Count() > 0)
                    {
                        var vLikes = _post.LikeStatues.ToList();

                        var vUsr = from lks in _post.LikeStatues.ToList()
                                   from usr in lks.Users.Where(x => x.UserName == _loginUser)
                                   select lks;

                        var _userLike = vUsr.FirstOrDefault();

                        if (_userLike != null)
                        {
                            isLike = true;

                            //for remove like
                            _post.LikeStatues.Remove(_userLike);

                            db.SaveChanges();

                            html = "Like";
                        }
                    }

                    if (isLike == false)
                    {
                        //for new like
                        List<User> lstUser = new List<User>();
                        lstUser.Add(_CurrUser);

                        int _pLkId = db.LikeStatues.Count();
                        _pLkId++;

                        LikeStatus _likeStatus = new LikeStatus();
                        _likeStatus.LikeId = "" + _pLkId;
                        _likeStatus.Users = lstUser;

                        List<LikeStatus> lstLike = new List<LikeStatus>();
                        lstLike.Add(_likeStatus);

                        //db.SaveChanges();

                        if (_post.LikeStatues == null)
                        {
                            _post.LikeStatues = new List<LikeStatus>() { _likeStatus };
                        }
                        else
                        {
                            _post.LikeStatues.Add(_likeStatus);
                        }

                        //_post.LikeStatues = lstLike;

                        db.Posts.Attach(_post);
                        db.Entry(_post).State = EntityState.Modified;

                        db.SaveChanges();

                        html = "UnLike";
                    }


                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "Donot insert";
            }

            return Json(new
            {
                Html = html,
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //CommentLikeSubmit
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CommentLikeSubmit(string pid, string cid)
        {
            string message = string.Empty;
            string html = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = Convert.ToInt32(pid);
            int _pComId = Convert.ToInt32(cid);

            Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

            PostComment _postComment = db.PostComments.Where(x => x.CommentId == _pComId).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (!String.IsNullOrEmpty(pid) && _postComment != null)
            {

                try
                {
                    bool isComLike = false;

                    if (_postComment.LikeStatues.Count() > 0)
                    {
                        var vLikes = _postComment.LikeStatues.ToList();

                        var vUsr = from comlks in _postComment.LikeStatues.ToList()
                                   from comusr in comlks.Users.Where(x => x.UserName == _loginUser)
                                   select comlks;

                        var _userLike = vUsr.FirstOrDefault();

                        if (_userLike != null)
                        {
                            isComLike = true;

                            //for remove like
                            _postComment.LikeStatues.Remove(_userLike);

                            db.SaveChanges();

                            html = "Like";
                        }
                    }

                    if (isComLike == false)
                    {
                        int _cLkId = db.LikeStatues.Count();
                        _cLkId++;

                        //for new like
                        List<User> lstUser = new List<User>();
                        lstUser.Add(_CurrUser);

                        LikeStatus _likeStatus = new LikeStatus();
                        _likeStatus.LikeId = "" + _cLkId;
                        _likeStatus.Users = lstUser;

                        List<LikeStatus> lstLike = new List<LikeStatus>();
                        lstLike.Add(_likeStatus);

                        _postComment.LikeStatues = lstLike;

                        db.PostComments.Attach(_postComment);
                        db.Entry(_postComment).State = EntityState.Modified;

                        db.SaveChanges();

                        html = "UnLike";
                    }


                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            else
            {
                message = "Donot insert";
            }

            return Json(new
            {
                Html = html,
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //PostLikeUpdate
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult PostLikeShowUpdate(string pid)
        {
            string message = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = Convert.ToInt32(pid);

            Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

            Post _viewPost = new Post();

            if (_post != null)
            {
                _viewPost.LikeStatues = _post.LikeStatues;
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostLikeShowUpdate", _viewPost),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //PostLikeUpdate
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult PostCommentShowUpdate(string pid)
        {
            string message = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = Convert.ToInt32(pid);

            Post _post = db.Posts.Where(x => x.PostId == _postId).FirstOrDefault();

            Post _viewPost = new Post();

            if (_post != null)
            {
                _viewPost.TotalComments = _post.TotalComments;
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostCommentShowUpdate", _viewPost),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //EventAttend
        [HttpGet]
        [ValidateInput(false)]
        public JsonResult PostEventAttend(int pid, int eid)
        {
            string message = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = pid;
            int _eventId = eid;

            Post _post = db.Posts.Where(x => x.PostId == _postId && x.EventPostId == _eventId).FirstOrDefault();

            return Json(new
            {
                Html = this.RenderPartialView("_PostEventAttend", _post),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }

        //EventAttend Submit
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostEventAttendSubmit(int pid, int eid, int eattId)
        {
            string message = string.Empty;

            string _loginUser = User.Identity.Name.ToString();

            int _postId = pid;
            int _eventId = eid;
            int _eventAttendId = eattId;

            Post _post = db.Posts.Where(x => x.PostId == _postId && x.EventPostId == _eventId).FirstOrDefault();

            EventPost _eventPost = _post.EventPost;

            EventAttendPerson _oldEventAttendPerson = _eventPost.EventAttendPersons.Where(x => x.EventPostId == _eventId && x.UserId == _loginUser).FirstOrDefault();

            User _CurrUser = db.Users.Where(x => x.UserName == _loginUser).FirstOrDefault();

            if (_eventPost != null)
            {

                bool isEvtAttend = false;

                if (_eventPost.EventAttendPersons.Count() > 0)
                {
                    //User vUsr = _oldEventAttendPerson.User;

                    if (_oldEventAttendPerson != null)
                    {
                        isEvtAttend = true;

                        //for remove like
                        _eventPost.EventAttendPersons.Remove(_oldEventAttendPerson);

                        db.SaveChanges();

                        EventAttendPerson _newUpdAttendPerson = new EventAttendPerson();
                        _newUpdAttendPerson.EventPostId = _eventPost.Id;
                        _newUpdAttendPerson.EventAttendTypeId = _eventAttendId;
                        _newUpdAttendPerson.UserId = _CurrUser.UserName;

                        _eventPost.EventAttendPersons.Add(_newUpdAttendPerson);

                        db.EventPosts.Attach(_eventPost);
                        db.Entry(_eventPost).State = EntityState.Modified;

                        db.SaveChanges();


                    }
                }

                if (isEvtAttend == false)
                {

                    EventAttendPerson _newCrtAttendPerson = new EventAttendPerson();
                    _newCrtAttendPerson.EventPostId = _eventPost.Id;
                    _newCrtAttendPerson.EventAttendTypeId = _eventAttendId;
                    _newCrtAttendPerson.UserId = _CurrUser.UserName;

                    _eventPost.EventAttendPersons.Add(_newCrtAttendPerson);

                    db.EventPosts.Attach(_eventPost);
                    db.Entry(_eventPost).State = EntityState.Modified;

                    db.SaveChanges();

                    _oldEventAttendPerson = _newCrtAttendPerson;

                }

            }
            else
            {
                message = "Donot insert";
            }

            return Json(new
            {
                Html = this.RenderPartialView("_PostEventAttendPerson", _eventPost),
                Message = message
            }, JsonRequestBehavior.AllowGet);

        }
    }
}
