<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MvcApplication2.Models.ViewModels.PersonListViewModel>" %>
<%@ Import Namespace="MvcApplication2.Models.DataModels"%>

<table>
  <thead>
    <tr>
      <th>
        First Name
      </th>
      <th>
        Last Name
      </th>
      <th></th>
    </tr>
  </thead>
  
  <tbody>
  <%int rowIndex = 0; %>
  <%foreach (Person person in Model.PersonList)
    { %>
      
    <tr class="item tablerow<%= (rowIndex % 2 == 0) ? "" : "_alt" %>">
      <td>
        <%= person.FirstName %>
      </td>
      <td>
        <%= person.LastName %>
      </td>
      <td align="right">
        <%= Ajax.ActionLink("delete", "JsonDelete", "People", new { Id = person.Id }, new AjaxOptions { Confirm = "Are you sure you want to Delete this Person? This action cannot be undone.", HttpMethod = "Delete", OnComplete = "JsonDelete_OnComplete" })%>
      </td>
    </tr>
      
    <% 
      rowIndex++; 
    } %>
  </tbody>
</table>