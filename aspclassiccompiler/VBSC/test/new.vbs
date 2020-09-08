imports system

dim sb

sb = new System.Text.StringBuilder()
sb.append("this")
sb.append(" is ")
sb.append(" stringbuilder!")
sb = sb + "asd" + "asd"
response.write sb.toString()
