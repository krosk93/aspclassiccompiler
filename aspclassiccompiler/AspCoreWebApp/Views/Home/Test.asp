<%
Option Explicit
Dim string1 : string1 = "abcdefc"
%>
<h3>Dim string1 : string1 = "abcdefc"</h3>
<p>InStrRev(string1, "c"): <%=InStrRev(string1, "c")%></p>
<p>InStrRev(string1, "c", -1): <%=InStrRev(string1, "c", -1)%></p>
<p>InStrRev(string1, "c", 1): <%=InStrRev(string1, "c", 1)%></p>
<p>InStrRev(string1, "c", 3): <%=InStrRev(string1, "c", 3)%></p>
<p>InStrRev(string1, "c", 5): <%=InStrRev(string1, "c", 5)%></p>
<p>InStrRev(string1, "c", 8): <%=InStrRev(string1, "c", 8)%></p>
<h3>Test case 1: string1 is ""</h3>
<% string1 = "" %>
<p>InStrRev(string1, "c"): <%=InStrRev(string1, "c")%></p>
<h3>Test case 2: string1 is null</h3>
<% string1 = vbNull %>
<p>InStrRev(string1, "c"): <%=InStrRev(string1, "c")%></p>