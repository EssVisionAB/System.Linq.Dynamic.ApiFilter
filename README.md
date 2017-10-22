# System.Linq.Dynamic.ApiFilter
Parse string filters to System.Linq.Dynamic 

 operands | sql equality 
-------  | ---------------------------|
 : | equal
 ~ | like
 < | smaler than
 <: | smaler than or equal
 \>  | greater than
 \>: | greater than or equal
 :(x,y,z) | inclusive or (WHER (x=x OR y=y OR z=>z)
