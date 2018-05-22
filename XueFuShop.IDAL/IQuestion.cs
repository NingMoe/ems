using System;
using XueFuShop.Models;
using System.Collections.Generic;

namespace XueFuShop.IDAL
{
    public interface IQuestion
    {
        QuestionInfo ReadQuestion(int Id);
        int AddQuestion(QuestionInfo Model);
        void UpdateQuestion(QuestionInfo Model);
        void DeleteQuestion(string IdStr);
        void DeleteQuestionByCateId(int CateId);
        void UpdateQuestionChecked(string Id, string Value);
        int ReadQuestionNum(string courseID);
        List<QuestionInfo> ReadList(int CateId);
        List<QuestionInfo> ReadList(QuestionInfo Model);
        List<QuestionInfo> ReadList(int currentPage, int pageSize, ref int count);
        List<QuestionInfo> ReadList(QuestionInfo Model, int currentPage, int pageSize, ref int count);
    }
}
