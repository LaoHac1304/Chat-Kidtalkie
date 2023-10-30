using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.QuestionRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> CreateQuestionAsync(QuestionCreateRequest questionViewModel)
        {
            var question = _mapper.Map<Question>(questionViewModel);   
            if (question == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "Question is null"
            });
            var result = await _questionRepository.InsertAsync(question);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(string.Format(CommandMessages.CreateFailed, question.Id))
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteQuestionAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(string.Format(CommandMessages.NotFound, id))
            });
            question.Status = 0;
            var result = await _questionRepository.UpdateAsync(question);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(string.Format(CommandMessages.DeleteFailed, question.Id))
            });
            return CommandResult.Success;

        }

        public async Task<QuestionViewModel> GetQuestionByIdAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            return _mapper.Map<QuestionViewModel>(question);
        }

        public async Task<(int, IEnumerable<QuestionViewModel>)> GetQuestionPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort)
        {
            IQueryable<Question> questions;
            string search = filterViewModel.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                questions = _questionRepository.TableNoTracking;
            }

            else
            {
                questions = _questionRepository.TableNoTracking
                                .Where(question => EF.Functions.ToTsVector("english", question.Answer + " " + question.Content + " " + question.Form)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (question => EF.Functions.ToTsVector("english", question.Answer + " " + question.Content + " " + question.Form)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }
            if (filterViewModel.Status != 2) questions = questions.Where(x => x.Status == filterViewModel.Status);
            if (!sort.IsNullOrEmpty())
            {
                questions = questions.Sort(sort);
            }
            var result = await questions.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (questions.Count(), _mapper.Map<List<QuestionViewModel>>(result));
        }

        public async Task<CommandResult> UpdateQuestionAsync(Guid id, QuestionUpdateRequest questionViewModel)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            _mapper.Map(questionViewModel, question);
            var result = await _questionRepository.UpdateAsync(question);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(string.Format(CommandMessages.UpdateFailed, id))
            });
            return CommandResult.Success;
        }
    }
}
