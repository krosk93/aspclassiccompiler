<%
option explicit

perftest	

sub perftest()
  dim loops
  dim counter
  dim tickes
  tickes = Timer()

  loops = 10000000
  counter = 0

  while loops > 0  
	  loops = loops - 1  
	  counter = counter + 1
  wend

  Response.Write("DoLoop result " & counter & "<BR/>")
  dim tickes2: tickes2 = Timer()
  Response.Write("Executing the while loop took: " & ((tickes2-tickes) * 100.0) & "ms" & "<BR/>")
  Response.Write("tickes: " & tickes & " tickes2: " & tickes2)
end sub

%>