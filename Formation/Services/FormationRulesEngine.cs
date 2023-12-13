using RulesEngine.Extensions;
using RulesEngine.Models;

namespace Formation.Services
{
    public class WorkflowsObject
    {
        public List<Workflow> Workflows { get; set; }
    }
    public interface IFormationRulesEngine<T>
    {
        Task<object> SubmitDataAsync<T>(T data);
    }
    public class FormationRulesEngine<T>: IFormationRulesEngine<T>
    {
        private readonly RulesEngine.RulesEngine _engine;
        public FormationRulesEngine(WorkflowsObject workflow)
        {
            new RulesEngine.RulesEngine(workflow.Workflows.ToArray(), null);
        }

        public async Task<object> SubmitDataAsync<T>(T data)
        {
            var result = await _engine.ExecuteAllRulesAsync("", data);
            result.OnSuccess(e => { });
            result.OnFail(() => { });
            return result;

        }
    }
}
