using System;

namespace demo_project.domain
{
    #region ParamsProcedure
    public abstract class BaseParamsProcedure
    {
    }

    /// <summary>
    /// lang_id
    /// </summary>
    public abstract class LangParamsProcedure : BaseParamsProcedure
    {
        public long lang_id { get; set; }
    }

    /// <summary>
    /// server_files, filepath, filename, lang_id
    /// </summary>
    public abstract class FileParamsProcedure : BaseParamsProcedure
    {
        public string server_files { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public long lang_id { get; set; }
    }

    /// <summary>
    ///  Id, user_id, lang_id
    /// </summary>
    public  class DeletedParamsProcedure : LangParamsProcedure
    {
        public long Id { get; set; }
        public long user_id { get; set; }
    }
    public class CustomerDeletedParamsProcedure : LangParamsProcedure
    {
        public long Id { get; set; }
        public long customer_id { get; set; }
    }

    /// <summary>
    /// list_id, user_id, lang_id
    /// </summary>
    public  class ListIdDeletedParamsProcedure : LangParamsProcedure
    {
        public string list_id { get; set; }
        public long user_id { get; set; }
    }
    #endregion
    #region Procedure
    public abstract class BaseProcedure
    {
    }

    /// <summary>
    /// result, id, message
    /// </summary>
    public abstract class ResultMessageProcedure : BaseProcedure
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }
    /// <inheritdoc/>
    public class AddNewProcedure : ResultMessageProcedure
    {
    }

    /// <inheritdoc/>
    public class CreateProcedure : ResultMessageProcedure
    {
    }

    /// <inheritdoc/>
    public class UpdatedProcedure : ResultMessageProcedure
    {
    }

    /// <inheritdoc/>
    public class DeletedProcedure : ResultMessageProcedure
    {
    }
    public class CheckProcedure : ResultMessageProcedure
    {
    }
    #endregion
}
