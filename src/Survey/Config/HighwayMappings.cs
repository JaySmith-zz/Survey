// [[Highway.Onramp.MVC.Data]]
// Copyright 2013 Timothy J. Rayburn
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Data.Entity;
using Highway.Data;
using Survey.Core.Mappings;

namespace Survey.Config
{
    public class HighwayMappings : IMappingConfiguration
    {
        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());

            modelBuilder.Configurations.Add(new SurveyMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new AnswerChoiceMap());
         
            modelBuilder.Configurations.Add(new QuestionnaireMap());
            modelBuilder.Configurations.Add(new ResponseMap());
            modelBuilder.Configurations.Add(new ResponseAuditMap());

            modelBuilder.Configurations.Add(new OnSiteReviewMap());
            modelBuilder.Configurations.Add(new OnSiteReviewCommentMap());
        }
    }
}