﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter
{
   // 'unified_to_softbank' => array(
			//"e29880"=>"ee818a", "\xe2\x98\x81"=>"ee8189", "\xe2\x98\x94"=>"\xee\x81\x8b", "\xe2\x9b\x84"=>"\xee\x81\x88",
			//"\xe2\x9a\xa1"=>"\xee\x84\xbd", "\xf0\x9f\x8c\x80"=>"\xee\x91\x83", "\xf0\x9f\x8c\x81"=>"[\xe9\x9c\xa7]", "\xf0\x9f\x8c\x82"=>"\xee\x90\xbc", "\xf0\x9f\x8c\x83"=>"\xee\x91\x8b",
			//"\xf0\x9f\x8c\x84"=>"\xee\x81\x8d", "\xf0\x9f\x8c\x85"=>"\xee\x91\x89", "\xf0\x9f\x8c\x86"=>"\xee\x85\x86", "\xf0\x9f\x8c\x87"=>"\xee\x91\x8a", "\xf0\x9f\x8c\x88"=>"\xee\x91\x8c",
			//"\xe2\x9d\x84"=>"[\xe9\x9b\xaa\xe7\xb5\x90\xe6\x99\xb6]", "\xe2\x9b\x85"=>"\xee\x81\x8a\xee\x81\x89", "\xf0\x9f\x8c\x89"=>"\xee\x91\x8b", "\xf0\x9f\x8c\x8a"=>"\xee\x90\xbe", "\xf0\x9f\x8c\x8b"=>"[\xe7\x81\xab\xe5\xb1\xb1]",
			//"\xf0\x9f\x8c\x8c"=>"\xee\x91\x8b", "\xf0\x9f\x8c\x8f"=>"[\xe5\x9c\xb0\xe7\x90\x83]", "\xf0\x9f\x8c\x91"=>"\xe2\x97\x8f", "\xf0\x9f\x8c\x94"=>"\xee\x81\x8c", "\xf0\x9f\x8c\x93"=>"\xee\x81\x8c",
			//"\xf0\x9f\x8c\x99"=>"\xee\x81\x8c", "\xf0\x9f\x8c\x95"=>"\xe2\x97\x8b", "\xf0\x9f\x8c\x9b"=>"\xee\x81\x8c", "\xf0\x9f\x8c\x9f"=>"\xee\x8c\xb5", "\xf0\x9f\x8c\xa0"=>"\xe2\x98\x86\xe5\xbd\xa1",
			//"\xf0\x9f\x95\x90"=>"\xee\x80\xa4", "\xf0\x9f\x95\x91"=>"\xee\x80\xa5", "\xf0\x9f\x95\x92"=>"\xee\x80\xa6", "\xf0\x9f\x95\x93"=>"\xee\x80\xa7", "\xf0\x9f\x95\x94"=>"\xee\x80\xa8",
			//"\xf0\x9f\x95\x95"=>"\xee\x80\xa9", "\xf0\x9f\x95\x96"=>"\xee\x80\xaa", "\xf0\x9f\x95\x97"=>"\xee\x80\xab", "\xf0\x9f\x95\x98"=>"\xee\x80\xac", "\xf0\x9f\x95\x99"=>"\xee\x80\xad",
			//"\xf0\x9f\x95\x9a"=>"\xee\x80\xae", "\xf0\x9f\x95\x9b"=>"\xee\x80\xaf", "\xe2\x8c\x9a"=>"[\xe8\x85\x95\xe6\x99\x82\xe8\xa8\x88]", "\xe2\x8c\x9b"=>"[\xe7\xa0\x82\xe6\x99\x82\xe8\xa8\x88]", "\xe2\x8f\xb0"=>"\xee\x80\xad",
			//"\xe2\x8f\xb3"=>"[\xe7\xa0\x82\xe6\x99\x82\xe8\xa8\x88]", "\xe2\x99\x88"=>"\xee\x88\xbf", "\xe2\x99\x89"=>"\xee\x89\x80", "\xe2\x99\x8a"=>"\xee\x89\x81", "\xe2\x99\x8b"=>"\xee\x89\x82",
			//"\xe2\x99\x8c"=>"\xee\x89\x83", "\xe2\x99\x8d"=>"\xee\x89\x84", "\xe2\x99\x8e"=>"\xee\x89\x85", "\xe2\x99\x8f"=>"\xee\x89\x86", "\xe2\x99\x90"=>"\xee\x89\x87",
			//"\xe2\x99\x91"=>"\xee\x89\x88", "\xe2\x99\x92"=>"\xee\x89\x89", "\xe2\x99\x93"=>"\xee\x89\x8a", "\xe2\x9b\x8e"=>"\xee\x89\x8b", "\xf0\x9f\x8d\x80"=>"\xee\x84\x90",
			//"\xf0\x9f\x8c\xb7"=>"\xee\x8c\x84", "\xf0\x9f\x8c\xb1"=>"\xee\x84\x90", "\xf0\x9f\x8d\x81"=>"\xee\x84\x98", "\xf0\x9f\x8c\xb8"=>"\xee\x80\xb0", "\xf0\x9f\x8c\xb9"=>"\xee\x80\xb2",
			//"\xf0\x9f\x8d\x82"=>"\xee\x84\x99", "\xf0\x9f\x8d\x83"=>"\xee\x91\x87", "\xf0\x9f\x8c\xba"=>"\xee\x8c\x83", "\xf0\x9f\x8c\xbb"=>"\xee\x8c\x85", "\xf0\x9f\x8c\xb4"=>"\xee\x8c\x87",
			//"\xf0\x9f\x8c\xb5"=>"\xee\x8c\x88", "\xf0\x9f\x8c\xbe"=>"\xee\x91\x84", "\xf0\x9f\x8c\xbd"=>"[\xe3\x81\xa8\xe3\x81\x86\xe3\x82\x82\xe3\x82\x8d\xe3\x81\x93\xe3\x81\x97]", "\xf0\x9f\x8d\x84"=>"[\xe3\x82\xad\xe3\x83\x8e\xe3\x82\xb3]", "\xf0\x9f\x8c\xb0"=>"[\xe6\xa0\x97]",
			//"\xf0\x9f\x8c\xbc"=>"\xee\x8c\x85", "\xf0\x9f\x8c\xbf"=>"\xee\x84\x90", "\xf0\x9f\x8d\x92"=>"[\xe3\x81\x95\xe3\x81\x8f\xe3\x82\x89\xe3\x82\x93\xe3\x81\xbc]", "\xf0\x9f\x8d\x8c"=>"[\xe3\x83\x90\xe3\x83\x8a\xe3\x83\x8a]", "\xf0\x9f\x8d\x8e"=>"\xee\x8d\x85",
			//"\xf0\x9f\x8d\x8a"=>"\xee\x8d\x86", "\xf0\x9f\x8d\x93"=>"\xee\x8d\x87", "\xf0\x9f\x8d\x89"=>"\xee\x8d\x88", "\xf0\x9f\x8d\x85"=>"\xee\x8d\x89", "\xf0\x9f\x8d\x86"=>"\xee\x8d\x8a",
			//"\xf0\x9f\x8d\x88"=>"[\xe3\x83\xa1\xe3\x83\xad\xe3\x83\xb3]", "\xf0\x9f\x8d\x8d"=>"[\xe3\x83\x91\xe3\x82\xa4\xe3\x83\x8a\xe3\x83\x83\xe3\x83\x97\xe3\x83\xab]", "\xf0\x9f\x8d\x87"=>"[\xe3\x83\x96\xe3\x83\x89\xe3\x82\xa6]", "\xf0\x9f\x8d\x91"=>"[\xe3\x83\xa2\xe3\x83\xa2]", "\xf0\x9f\x8d\x8f"=>"\xee\x8d\x85",
			//"\xf0\x9f\x91\x80"=>"\xee\x90\x99", "\xf0\x9f\x91\x82"=>"\xee\x90\x9b", "\xf0\x9f\x91\x83"=>"\xee\x90\x9a", "\xf0\x9f\x91\x84"=>"\xee\x90\x9c", "\xf0\x9f\x91\x85"=>"\xee\x90\x89",
			//"\xf0\x9f\x92\x84"=>"\xee\x8c\x9c", "\xf0\x9f\x92\x85"=>"\xee\x8c\x9d", "\xf0\x9f\x92\x86"=>"\xee\x8c\x9e", "\xf0\x9f\x92\x87"=>"\xee\x8c\x9f", "\xf0\x9f\x92\x88"=>"\xee\x8c\xa0",
			//"\xf0\x9f\x91\xa4"=>"\xe3\x80\x93", "\xf0\x9f\x91\xa6"=>"\xee\x80\x81", "\xf0\x9f\x91\xa7"=>"\xee\x80\x82", "\xf0\x9f\x91\xa8"=>"\xee\x80\x84", "\xf0\x9f\x91\xa9"=>"\xee\x80\x85",
			//"\xf0\x9f\x91\xaa"=>"[\xe5\xae\xb6\xe6\x97\x8f]", "\xf0\x9f\x91\xab"=>"\xee\x90\xa8", "\xf0\x9f\x91\xae"=>"\xee\x85\x92", "\xf0\x9f\x91\xaf"=>"\xee\x90\xa9", "\xf0\x9f\x91\xb0"=>"[\xe8\x8a\xb1\xe5\xab\x81]",
			//"\xf0\x9f\x91\xb1"=>"\xee\x94\x95", "\xf0\x9f\x91\xb2"=>"\xee\x94\x96", "\xf0\x9f\x91\xb3"=>"\xee\x94\x97", "\xf0\x9f\x91\xb4"=>"\xee\x94\x98", "\xf0\x9f\x91\xb5"=>"\xee\x94\x99",
			//"\xf0\x9f\x91\xb6"=>"\xee\x94\x9a", "\xf0\x9f\x91\xb7"=>"\xee\x94\x9b", "\xf0\x9f\x91\xb8"=>"\xee\x94\x9c", "\xf0\x9f\x91\xb9"=>"[\xe3\x81\xaa\xe3\x81\xbe\xe3\x81\xaf\xe3\x81\x92]", "\xf0\x9f\x91\xba"=>"[\xe5\xa4\xa9\xe7\x8b\x97]",
			//"\xf0\x9f\x91\xbb"=>"\xee\x84\x9b", "\xf0\x9f\x91\xbc"=>"\xee\x81\x8e", "\xf0\x9f\x91\xbd"=>"\xee\x84\x8c", "\xf0\x9f\x91\xbe"=>"\xee\x84\xab", "\xf0\x9f\x91\xbf"=>"\xee\x84\x9a",
			//"\xf0\x9f\x92\x80"=>"\xee\x84\x9c", "\xf0\x9f\x92\x81"=>"\xee\x89\x93", "\xf0\x9f\x92\x82"=>"\xee\x94\x9e", "\xf0\x9f\x92\x83"=>"\xee\x94\x9f", "\xf0\x9f\x90\x8c"=>"[\xe3\x82\xab\xe3\x82\xbf\xe3\x83\x84\xe3\x83\xa0\xe3\x83\xaa]",
			//"\xf0\x9f\x90\x8d"=>"\xee\x94\xad", "\xf0\x9f\x90\x8e"=>"\xee\x84\xb4", "\xf0\x9f\x90\x94"=>"\xee\x94\xae", "\xf0\x9f\x90\x97"=>"\xee\x94\xaf", "\xf0\x9f\x90\xab"=>"\xee\x94\xb0",
			//"\xf0\x9f\x90\x98"=>"\xee\x94\xa6", "\xf0\x9f\x90\xa8"=>"\xee\x94\xa7", "\xf0\x9f\x90\x92"=>"\xee\x94\xa8", "\xf0\x9f\x90\x91"=>"\xee\x94\xa9", "\xf0\x9f\x90\x99"=>"\xee\x84\x8a",
			//"\xf0\x9f\x90\x9a"=>"\xee\x91\x81", "\xf0\x9f\x90\x9b"=>"\xee\x94\xa5", "\xf0\x9f\x90\x9c"=>"[\xe3\x82\xa2\xe3\x83\xaa]", "\xf0\x9f\x90\x9d"=>"[\xe3\x83\x9f\xe3\x83\x84\xe3\x83\x90\xe3\x83\x81]", "\xf0\x9f\x90\x9e"=>"[\xe3\x81\xa6\xe3\x82\x93\xe3\x81\xa8\xe3\x81\x86\xe8\x99\xab]",
			//"\xf0\x9f\x90\xa0"=>"\xee\x94\xa2", "\xf0\x9f\x90\xa1"=>"\xee\x80\x99", "\xf0\x9f\x90\xa2"=>"[\xe3\x82\xab\xe3\x83\xa1]", "\xf0\x9f\x90\xa4"=>"\xee\x94\xa3", "\xf0\x9f\x90\xa5"=>"\xee\x94\xa3",
			//"\xf0\x9f\x90\xa6"=>"\xee\x94\xa1", "\xf0\x9f\x90\xa3"=>"\xee\x94\xa3", "\xf0\x9f\x90\xa7"=>"\xee\x81\x95", "\xf0\x9f\x90\xa9"=>"\xee\x81\x92", "\xf0\x9f\x90\x9f"=>"\xee\x80\x99",
			//"\xf0\x9f\x90\xac"=>"\xee\x94\xa0", "\xf0\x9f\x90\xad"=>"\xee\x81\x93", "\xf0\x9f\x90\xaf"=>"\xee\x81\x90", "\xf0\x9f\x90\xb1"=>"\xee\x81\x8f", "\xf0\x9f\x90\xb3"=>"\xee\x81\x94",
			//"\xf0\x9f\x90\xb4"=>"\xee\x80\x9a", "\xf0\x9f\x90\xb5"=>"\xee\x84\x89", "\xf0\x9f\x90\xb6"=>"\xee\x81\x92", "\xf0\x9f\x90\xb7"=>"\xee\x84\x8b", "\xf0\x9f\x90\xbb"=>"\xee\x81\x91",
			//"\xf0\x9f\x90\xb9"=>"\xee\x94\xa4", "\xf0\x9f\x90\xba"=>"\xee\x94\xaa", "\xf0\x9f\x90\xae"=>"\xee\x94\xab", "\xf0\x9f\x90\xb0"=>"\xee\x94\xac", "\xf0\x9f\x90\xb8"=>"\xee\x94\xb1",
			//"\xf0\x9f\x90\xbe"=>"\xee\x94\xb6", "\xf0\x9f\x90\xb2"=>"[\xe8\xbe\xb0]", "\xf0\x9f\x90\xbc"=>"[\xe3\x83\x91\xe3\x83\xb3\xe3\x83\x80]", "\xf0\x9f\x90\xbd"=>"\xee\x84\x8b", "\xf0\x9f\x98\xa0"=>"\xee\x81\x99",
			//"\xf0\x9f\x98\xa9"=>"\xee\x90\x83", "\xf0\x9f\x98\xb2"=>"\xee\x90\x90", "\xf0\x9f\x98\x9e"=>"\xee\x81\x98", "\xf0\x9f\x98\xb5"=>"\xee\x90\x86", "\xf0\x9f\x98\xb0"=>"\xee\x90\x8f",
			//"\xf0\x9f\x98\x92"=>"\xee\x90\x8e", "\xf0\x9f\x98\x8d"=>"\xee\x84\x86", "\xf0\x9f\x98\xa4"=>"\xee\x90\x84", "\xf0\x9f\x98\x9c"=>"\xee\x84\x85", "\xf0\x9f\x98\x9d"=>"\xee\x90\x89",
			//"\xf0\x9f\x98\x8b"=>"\xee\x81\x96", "\xf0\x9f\x98\x98"=>"\xee\x90\x98", "\xf0\x9f\x98\x9a"=>"\xee\x90\x97", "\xf0\x9f\x98\xb7"=>"\xee\x90\x8c", "\xf0\x9f\x98\xb3"=>"\xee\x90\x8d",
			//"\xf0\x9f\x98\x83"=>"\xee\x81\x97", "\xf0\x9f\x98\x85"=>"\xee\x90\x95\xee\x8c\xb1", "\xf0\x9f\x98\x86"=>"\xee\x90\x8a", "\xf0\x9f\x98\x81"=>"\xee\x90\x84", "\xf0\x9f\x98\x82"=>"\xee\x90\x92",
			//"\xf0\x9f\x98\x8a"=>"\xee\x81\x96", "\xe2\x98\xba"=>"\xee\x90\x94", "\xf0\x9f\x98\x84"=>"\xee\x90\x95", "\xf0\x9f\x98\xa2"=>"\xee\x90\x93", "\xf0\x9f\x98\xad"=>"\xee\x90\x91",
			//"\xf0\x9f\x98\xa8"=>"\xee\x90\x8b", "\xf0\x9f\x98\xa3"=>"\xee\x90\x86", "\xf0\x9f\x98\xa1"=>"\xee\x90\x96", "\xf0\x9f\x98\x8c"=>"\xee\x90\x8a", "\xf0\x9f\x98\x96"=>"\xee\x90\x87",
			//"\xf0\x9f\x98\x94"=>"\xee\x90\x83", "\xf0\x9f\x98\xb1"=>"\xee\x84\x87", "\xf0\x9f\x98\xaa"=>"\xee\x90\x88", "\xf0\x9f\x98\x8f"=>"\xee\x90\x82", "\xf0\x9f\x98\x93"=>"\xee\x84\x88",
			//"\xf0\x9f\x98\xa5"=>"\xee\x90\x81", "\xf0\x9f\x98\xab"=>"\xee\x90\x86", "\xf0\x9f\x98\x89"=>"\xee\x90\x85", "\xf0\x9f\x98\xba"=>"\xee\x81\x97", "\xf0\x9f\x98\xb8"=>"\xee\x90\x84",
			//"\xf0\x9f\x98\xb9"=>"\xee\x90\x92", "\xf0\x9f\x98\xbd"=>"\xee\x90\x98", "\xf0\x9f\x98\xbb"=>"\xee\x84\x86", "\xf0\x9f\x98\xbf"=>"\xee\x90\x93", "\xf0\x9f\x98\xbe"=>"\xee\x90\x96",
			//"\xf0\x9f\x98\xbc"=>"\xee\x90\x84", "\xf0\x9f\x99\x80"=>"\xee\x90\x83", "\xf0\x9f\x99\x85"=>"\xee\x90\xa3", "\xf0\x9f\x99\x86"=>"\xee\x90\xa4", "\xf0\x9f\x99\x87"=>"\xee\x90\xa6",
			//"\xf0\x9f\x99\x88"=>"(/_\xef\xbc\xbc)", "\xf0\x9f\x99\x8a"=>"(\xe3\x83\xbb\xc3\x97\xe3\x83\xbb)", "\xf0\x9f\x99\x89"=>"|(\xe3\x83\xbb\xc3\x97\xe3\x83\xbb)|", "\xf0\x9f\x99\x8b"=>"\xee\x80\x92", "\xf0\x9f\x99\x8c"=>"\xee\x90\xa7",
			//"\xf0\x9f\x99\x8d"=>"\xee\x90\x83", "\xf0\x9f\x99\x8e"=>"\xee\x90\x96", "\xf0\x9f\x99\x8f"=>"\xee\x90\x9d", "\xf0\x9f\x8f\xa0"=>"\xee\x80\xb6", "\xf0\x9f\x8f\xa1"=>"\xee\x80\xb6",
			//"\xf0\x9f\x8f\xa2"=>"\xee\x80\xb8", "\xf0\x9f\x8f\xa3"=>"\xee\x85\x93", "\xf0\x9f\x8f\xa5"=>"\xee\x85\x95", "\xf0\x9f\x8f\xa6"=>"\xee\x85\x8d", "\xf0\x9f\x8f\xa7"=>"\xee\x85\x94",
			//"\xf0\x9f\x8f\xa8"=>"\xee\x85\x98", "\xf0\x9f\x8f\xa9"=>"\xee\x94\x81", "\xf0\x9f\x8f\xaa"=>"\xee\x85\x96", "\xf0\x9f\x8f\xab"=>"\xee\x85\x97", "\xe2\x9b\xaa"=>"\xee\x80\xb7",
			//"\xe2\x9b\xb2"=>"\xee\x84\xa1", "\xf0\x9f\x8f\xac"=>"\xee\x94\x84", "\xf0\x9f\x8f\xaf"=>"\xee\x94\x85", "\xf0\x9f\x8f\xb0"=>"\xee\x94\x86", "\xf0\x9f\x8f\xad"=>"\xee\x94\x88",
			//"\xe2\x9a\x93"=>"\xee\x88\x82", "\xf0\x9f\x8f\xae"=>"\xee\x8c\x8b", "\xf0\x9f\x97\xbb"=>"\xee\x80\xbb", "\xf0\x9f\x97\xbc"=>"\xee\x94\x89", "\xf0\x9f\x97\xbd"=>"\xee\x94\x9d",
			//"\xf0\x9f\x97\xbe"=>"[\xe6\x97\xa5\xe6\x9c\xac\xe5\x9c\xb0\xe5\x9b\xb3]", "\xf0\x9f\x97\xbf"=>"[\xe3\x83\xa2\xe3\x82\xa2\xe3\x82\xa4]", "\xf0\x9f\x91\x9e"=>"\xee\x80\x87", "\xf0\x9f\x91\x9f"=>"\xee\x80\x87", "\xf0\x9f\x91\xa0"=>"\xee\x84\xbe",
			//"\xf0\x9f\x91\xa1"=>"\xee\x8c\x9a", "\xf0\x9f\x91\xa2"=>"\xee\x8c\x9b", "\xf0\x9f\x91\xa3"=>"\xee\x94\xb6", "\xf0\x9f\x91\x93"=>"[\xe3\x83\xa1\xe3\x82\xac\xe3\x83\x8d]", "\xf0\x9f\x91\x95"=>"\xee\x80\x86",
			//"\xf0\x9f\x91\x96"=>"[\xe3\x82\xb8\xe3\x83\xbc\xe3\x83\xb3\xe3\x82\xba]", "\xf0\x9f\x91\x91"=>"\xee\x84\x8e", "\xf0\x9f\x91\x94"=>"\xee\x8c\x82", "\xf0\x9f\x91\x92"=>"\xee\x8c\x98", "\xf0\x9f\x91\x97"=>"\xee\x8c\x99",
			//"\xf0\x9f\x91\x98"=>"\xee\x8c\xa1", "\xf0\x9f\x91\x99"=>"\xee\x8c\xa2", "\xf0\x9f\x91\x9a"=>"\xee\x80\x86", "\xf0\x9f\x91\x9b"=>"[\xe8\xb2\xa1\xe5\xb8\x83]", "\xf0\x9f\x91\x9c"=>"\xee\x8c\xa3",
			//"\xf0\x9f\x91\x9d"=>"[\xe3\x81\xb5\xe3\x81\x8f\xe3\x82\x8d]", "\xf0\x9f\x92\xb0"=>"\xee\x84\xaf", "\xf0\x9f\x92\xb1"=>"\xee\x85\x89", "\xf0\x9f\x92\xb9"=>"\xee\x85\x8a", "\xf0\x9f\x92\xb2"=>"\xee\x84\xaf",
			//"\xf0\x9f\x92\xb3"=>"[\xe3\x82\xab\xe3\x83\xbc\xe3\x83\x89]", "\xf0\x9f\x92\xb4"=>"\xef\xbf\xa5", "\xf0\x9f\x92\xb5"=>"\xee\x84\xaf", "\xf0\x9f\x92\xb8"=>"[\xe9\xa3\x9b\xe3\x82\x93\xe3\x81\xa7\xe3\x81\x84\xe3\x81\x8f\xe3\x81\x8a\xe9\x87\x91]", "\xf0\x9f\x87\xa8\xf0\x9f\x87\xb3"=>"\xee\x94\x93",
			//"\xf0\x9f\x87\xa9\xf0\x9f\x87\xaa"=>"\xee\x94\x8e", "\xf0\x9f\x87\xaa\xf0\x9f\x87\xb8"=>"\xee\x94\x91", "\xf0\x9f\x87\xab\xf0\x9f\x87\xb7"=>"\xee\x94\x8d", "\xf0\x9f\x87\xac\xf0\x9f\x87\xa7"=>"\xee\x94\x90", "\xf0\x9f\x87\xae\xf0\x9f\x87\xb9"=>"\xee\x94\x8f",
			//"\xf0\x9f\x87\xaf\xf0\x9f\x87\xb5"=>"\xee\x94\x8b", "\xf0\x9f\x87\xb0\xf0\x9f\x87\xb7"=>"\xee\x94\x94", "\xf0\x9f\x87\xb7\xf0\x9f\x87\xba"=>"\xee\x94\x92", "\xf0\x9f\x87\xba\xf0\x9f\x87\xb8"=>"\xee\x94\x8c", "\xf0\x9f\x94\xa5"=>"\xee\x84\x9d",
			//"\xf0\x9f\x94\xa6"=>"[\xe6\x87\x90\xe4\xb8\xad\xe9\x9b\xbb\xe7\x81\xaf]", "\xf0\x9f\x94\xa7"=>"[\xe3\x83\xac\xe3\x83\xb3\xe3\x83\x81]", "\xf0\x9f\x94\xa8"=>"\xee\x84\x96", "\xf0\x9f\x94\xa9"=>"[\xe3\x83\x8d\xe3\x82\xb8]", "\xf0\x9f\x94\xaa"=>"[\xe5\x8c\x85\xe4\xb8\x81]",
			//"\xf0\x9f\x94\xab"=>"\xee\x84\x93", "\xf0\x9f\x94\xae"=>"\xee\x88\xbe", "\xf0\x9f\x94\xaf"=>"\xee\x88\xbe", "\xf0\x9f\x94\xb0"=>"\xee\x88\x89", "\xf0\x9f\x94\xb1"=>"\xee\x80\xb1",
			//"\xf0\x9f\x92\x89"=>"\xee\x84\xbb", "\xf0\x9f\x92\x8a"=>"\xee\x8c\x8f", "\xf0\x9f\x85\xb0"=>"\xee\x94\xb2", "\xf0\x9f\x85\xb1"=>"\xee\x94\xb3", "\xf0\x9f\x86\x8e"=>"\xee\x94\xb4",
			//"\xf0\x9f\x85\xbe"=>"\xee\x94\xb5", "\xf0\x9f\x8e\x80"=>"\xee\x8c\x94", "\xf0\x9f\x8e\x81"=>"\xee\x84\x92", "\xf0\x9f\x8e\x82"=>"\xee\x8d\x8b", "\xf0\x9f\x8e\x84"=>"\xee\x80\xb3",
			//"\xf0\x9f\x8e\x85"=>"\xee\x91\x88", "\xf0\x9f\x8e\x8c"=>"\xee\x85\x83", "\xf0\x9f\x8e\x86"=>"\xee\x84\x97", "\xf0\x9f\x8e\x88"=>"\xee\x8c\x90", "\xf0\x9f\x8e\x89"=>"\xee\x8c\x92",
			//"\xf0\x9f\x8e\x8d"=>"\xee\x90\xb6", "\xf0\x9f\x8e\x8e"=>"\xee\x90\xb8", "\xf0\x9f\x8e\x93"=>"\xee\x90\xb9", "\xf0\x9f\x8e\x92"=>"\xee\x90\xba", "\xf0\x9f\x8e\x8f"=>"\xee\x90\xbb",
			//"\xf0\x9f\x8e\x87"=>"\xee\x91\x80", "\xf0\x9f\x8e\x90"=>"\xee\x91\x82", "\xf0\x9f\x8e\x83"=>"\xee\x91\x85", "\xf0\x9f\x8e\x8a"=>"[\xe3\x82\xaa\xe3\x83\xa1\xe3\x83\x87\xe3\x83\x88\xe3\x82\xa6]", "\xf0\x9f\x8e\x8b"=>"[\xe4\xb8\x83\xe5\xa4\x95]",
			//"\xf0\x9f\x8e\x91"=>"\xee\x91\x86", "\xf0\x9f\x93\x9f"=>"[\xe3\x83\x9d\xe3\x82\xb1\xe3\x83\x99\xe3\x83\xab]", "\xe2\x98\x8e"=>"\xee\x80\x89", "\xf0\x9f\x93\x9e"=>"\xee\x80\x89", "\xf0\x9f\x93\xb1"=>"\xee\x80\x8a",
			//"\xf0\x9f\x93\xb2"=>"\xee\x84\x84", "\xf0\x9f\x93\x9d"=>"\xee\x8c\x81", "\xf0\x9f\x93\xa0"=>"\xee\x80\x8b", "\xe2\x9c\x89"=>"\xee\x84\x83", "\xf0\x9f\x93\xa8"=>"\xee\x84\x83",
			//"\xf0\x9f\x93\xa9"=>"\xee\x84\x83", "\xf0\x9f\x93\xaa"=>"\xee\x84\x81", "\xf0\x9f\x93\xab"=>"\xee\x84\x81", "\xf0\x9f\x93\xae"=>"\xee\x84\x82", "\xf0\x9f\x93\xb0"=>"[\xe6\x96\xb0\xe8\x81\x9e]",
			//"\xf0\x9f\x93\xa2"=>"\xee\x85\x82", "\xf0\x9f\x93\xa3"=>"\xee\x8c\x97", "\xf0\x9f\x93\xa1"=>"\xee\x85\x8b", "\xf0\x9f\x93\xa4"=>"[\xe9\x80\x81\xe4\xbf\xa1BOX]", "\xf0\x9f\x93\xa5"=>"[\xe5\x8f\x97\xe4\xbf\xa1BOX]",
			//"\xf0\x9f\x93\xa6"=>"\xee\x84\x92", "\xf0\x9f\x93\xa7"=>"\xee\x84\x83", "\xf0\x9f\x94\xa0"=>"[ABCD]", "\xf0\x9f\x94\xa1"=>"[abcd]", "\xf0\x9f\x94\xa2"=>"[1234]",
			//"\xf0\x9f\x94\xa3"=>"[\xe8\xa8\x98\xe5\x8f\xb7]", "\xf0\x9f\x94\xa4"=>"[ABC]", "\xe2\x9c\x92"=>"[\xe3\x83\x9a\xe3\x83\xb3]", "\xf0\x9f\x92\xba"=>"\xee\x84\x9f", "\xf0\x9f\x92\xbb"=>"\xee\x80\x8c",
			//"\xe2\x9c\x8f"=>"\xee\x8c\x81", "\xf0\x9f\x93\x8e"=>"[\xe3\x82\xaf\xe3\x83\xaa\xe3\x83\x83\xe3\x83\x97]", "\xf0\x9f\x92\xbc"=>"\xee\x84\x9e", "\xf0\x9f\x92\xbd"=>"\xee\x8c\x96", "\xf0\x9f\x92\xbe"=>"\xee\x8c\x96",
			//"\xf0\x9f\x92\xbf"=>"\xee\x84\xa6", "\xf0\x9f\x93\x80"=>"\xee\x84\xa7", "\xe2\x9c\x82"=>"\xee\x8c\x93", "\xf0\x9f\x93\x8d"=>"[\xe7\x94\xbb\xe3\x81\xb3\xe3\x82\x87\xe3\x81\x86]", "\xf0\x9f\x93\x83"=>"\xee\x8c\x81",
			//"\xf0\x9f\x93\x84"=>"\xee\x8c\x81", "\xf0\x9f\x93\x85"=>"[\xe3\x82\xab\xe3\x83\xac\xe3\x83\xb3\xe3\x83\x80\xe3\x83\xbc]", "\xf0\x9f\x93\x81"=>"[\xe3\x83\x95\xe3\x82\xa9\xe3\x83\xab\xe3\x83\x80]", "\xf0\x9f\x93\x82"=>"[\xe3\x83\x95\xe3\x82\xa9\xe3\x83\xab\xe3\x83\x80]", "\xf0\x9f\x93\x93"=>"\xee\x85\x88",
			//"\xf0\x9f\x93\x96"=>"\xee\x85\x88", "\xf0\x9f\x93\x94"=>"\xee\x85\x88", "\xf0\x9f\x93\x95"=>"\xee\x85\x88", "\xf0\x9f\x93\x97"=>"\xee\x85\x88", "\xf0\x9f\x93\x98"=>"\xee\x85\x88",
			//"\xf0\x9f\x93\x99"=>"\xee\x85\x88", "\xf0\x9f\x93\x9a"=>"\xee\x85\x88", "\xf0\x9f\x93\x9b"=>"[\xe5\x90\x8d\xe6\x9c\xad]", "\xf0\x9f\x93\x9c"=>"[\xe3\x82\xb9\xe3\x82\xaf\xe3\x83\xad\xe3\x83\xbc\xe3\x83\xab]", "\xf0\x9f\x93\x8b"=>"\xee\x8c\x81",
			//"\xf0\x9f\x93\x86"=>"[\xe3\x82\xab\xe3\x83\xac\xe3\x83\xb3\xe3\x83\x80\xe3\x83\xbc]", "\xf0\x9f\x93\x8a"=>"\xee\x85\x8a", "\xf0\x9f\x93\x88"=>"\xee\x85\x8a", "\xf0\x9f\x93\x89"=>"[\xe3\x82\xb0\xe3\x83\xa9\xe3\x83\x95]", "\xf0\x9f\x93\x87"=>"\xee\x85\x88",
			//"\xf0\x9f\x93\x8c"=>"[\xe7\x94\xbb\xe3\x81\xb3\xe3\x82\x87\xe3\x81\x86]", "\xf0\x9f\x93\x92"=>"\xee\x85\x88", "\xf0\x9f\x93\x8f"=>"[\xe5\xae\x9a\xe8\xa6\x8f]", "\xf0\x9f\x93\x90"=>"[\xe4\xb8\x89\xe8\xa7\x92\xe5\xae\x9a\xe8\xa6\x8f]", "\xf0\x9f\x93\x91"=>"\xee\x8c\x81",
			//"\xf0\x9f\x8e\xbd"=>"\xe3\x80\x93", "\xe2\x9a\xbe"=>"\xee\x80\x96", "\xe2\x9b\xb3"=>"\xee\x80\x94", "\xf0\x9f\x8e\xbe"=>"\xee\x80\x95", "\xe2\x9a\xbd"=>"\xee\x80\x98",
			//"\xf0\x9f\x8e\xbf"=>"\xee\x80\x93", "\xf0\x9f\x8f\x80"=>"\xee\x90\xaa", "\xf0\x9f\x8f\x81"=>"\xee\x84\xb2", "\xf0\x9f\x8f\x82"=>"[\xe3\x82\xb9\xe3\x83\x8e\xe3\x83\x9c]", "\xf0\x9f\x8f\x83"=>"\xee\x84\x95",
			//"\xf0\x9f\x8f\x84"=>"\xee\x80\x97", "\xf0\x9f\x8f\x86"=>"\xee\x84\xb1", "\xf0\x9f\x8f\x88"=>"\xee\x90\xab", "\xf0\x9f\x8f\x8a"=>"\xee\x90\xad", "\xf0\x9f\x9a\x83"=>"\xee\x80\x9e",
			//"\xf0\x9f\x9a\x87"=>"\xee\x90\xb4", "\xe2\x93\x82"=>"\xee\x90\xb4", "\xf0\x9f\x9a\x84"=>"\xee\x90\xb5", "\xf0\x9f\x9a\x85"=>"\xee\x80\x9f", "\xf0\x9f\x9a\x97"=>"\xee\x80\x9b",
			//"\xf0\x9f\x9a\x99"=>"\xee\x90\xae", "\xf0\x9f\x9a\x8c"=>"\xee\x85\x99", "\xf0\x9f\x9a\x8f"=>"\xee\x85\x90", "\xf0\x9f\x9a\xa2"=>"\xee\x88\x82", "\xe2\x9c\x88"=>"\xee\x80\x9d",
			//"\xe2\x9b\xb5"=>"\xee\x80\x9c", "\xf0\x9f\x9a\x89"=>"\xee\x80\xb9", "\xf0\x9f\x9a\x80"=>"\xee\x84\x8d", "\xf0\x9f\x9a\xa4"=>"\xee\x84\xb5", "\xf0\x9f\x9a\x95"=>"\xee\x85\x9a",
			//"\xf0\x9f\x9a\x9a"=>"\xee\x90\xaf", "\xf0\x9f\x9a\x92"=>"\xee\x90\xb0", "\xf0\x9f\x9a\x91"=>"\xee\x90\xb1", "\xf0\x9f\x9a\x93"=>"\xee\x90\xb2", "\xe2\x9b\xbd"=>"\xee\x80\xba",
			//"\xf0\x9f\x85\xbf"=>"\xee\x85\x8f", "\xf0\x9f\x9a\xa5"=>"\xee\x85\x8e", "\xf0\x9f\x9a\xa7"=>"\xee\x84\xb7", "\xf0\x9f\x9a\xa8"=>"\xee\x90\xb2", "\xe2\x99\xa8"=>"\xee\x84\xa3",
			//"\xe2\x9b\xba"=>"\xee\x84\xa2", "\xf0\x9f\x8e\xa0"=>"\xe3\x80\x93", "\xf0\x9f\x8e\xa1"=>"\xee\x84\xa4", "\xf0\x9f\x8e\xa2"=>"\xee\x90\xb3", "\xf0\x9f\x8e\xa3"=>"\xee\x80\x99",
			//"\xf0\x9f\x8e\xa4"=>"\xee\x80\xbc", "\xf0\x9f\x8e\xa5"=>"\xee\x80\xbd", "\xf0\x9f\x8e\xa6"=>"\xee\x94\x87", "\xf0\x9f\x8e\xa7"=>"\xee\x8c\x8a", "\xf0\x9f\x8e\xa8"=>"\xee\x94\x82",
			//"\xf0\x9f\x8e\xa9"=>"\xee\x94\x83", "\xf0\x9f\x8e\xaa"=>"[\xe3\x82\xa4\xe3\x83\x99\xe3\x83\xb3\xe3\x83\x88]", "\xf0\x9f\x8e\xab"=>"\xee\x84\xa5", "\xf0\x9f\x8e\xac"=>"\xee\x8c\xa4", "\xf0\x9f\x8e\xad"=>"\xee\x94\x83",
			//"\xf0\x9f\x8e\xae"=>"[\xe3\x82\xb2\xe3\x83\xbc\xe3\x83\xa0]", "\xf0\x9f\x80\x84"=>"\xee\x84\xad", "\xf0\x9f\x8e\xaf"=>"\xee\x84\xb0", "\xf0\x9f\x8e\xb0"=>"\xee\x84\xb3", "\xf0\x9f\x8e\xb1"=>"\xee\x90\xac",
			//"\xf0\x9f\x8e\xb2"=>"[\xe3\x82\xb5\xe3\x82\xa4\xe3\x82\xb3\xe3\x83\xad]", "\xf0\x9f\x8e\xb3"=>"[\xe3\x83\x9c\xe3\x83\xbc\xe3\x83\xaa\xe3\x83\xb3\xe3\x82\xb0]", "\xf0\x9f\x8e\xb4"=>"[\xe8\x8a\xb1\xe6\x9c\xad]", "\xf0\x9f\x83\x8f"=>"[\xe3\x82\xb8\xe3\x83\xa7\xe3\x83\xbc\xe3\x82\xab\xe3\x83\xbc]", "\xf0\x9f\x8e\xb5"=>"\xee\x80\xbe",
			//"\xf0\x9f\x8e\xb6"=>"\xee\x8c\xa6", "\xf0\x9f\x8e\xb7"=>"\xee\x81\x80", "\xf0\x9f\x8e\xb8"=>"\xee\x81\x81", "\xf0\x9f\x8e\xb9"=>"[\xe3\x83\x94\xe3\x82\xa2\xe3\x83\x8e]", "\xf0\x9f\x8e\xba"=>"\xee\x81\x82",
			//"\xf0\x9f\x8e\xbb"=>"[\xe3\x83\x90\xe3\x82\xa4\xe3\x82\xaa\xe3\x83\xaa\xe3\x83\xb3]", "\xf0\x9f\x8e\xbc"=>"\xee\x8c\xa6", "\xe3\x80\xbd"=>"\xee\x84\xac", "\xf0\x9f\x93\xb7"=>"\xee\x80\x88", "\xf0\x9f\x93\xb9"=>"\xee\x80\xbd",
			//"\xf0\x9f\x93\xba"=>"\xee\x84\xaa", "\xf0\x9f\x93\xbb"=>"\xee\x84\xa8", "\xf0\x9f\x93\xbc"=>"\xee\x84\xa9", "\xf0\x9f\x92\x8b"=>"\xee\x80\x83", "\xf0\x9f\x92\x8c"=>"\xee\x84\x83\xee\x8c\xa8",
			//"\xf0\x9f\x92\x8d"=>"\xee\x80\xb4", "\xf0\x9f\x92\x8e"=>"\xee\x80\xb5", "\xf0\x9f\x92\x8f"=>"\xee\x84\x91", "\xf0\x9f\x92\x90"=>"\xee\x8c\x86", "\xf0\x9f\x92\x91"=>"\xee\x90\xa5",
			//"\xf0\x9f\x92\x92"=>"\xee\x90\xbd", "\xf0\x9f\x94\x9e"=>"\xee\x88\x87", "\xc2\xa9"=>"\xee\x89\x8e", "\xc2\xae"=>"\xee\x89\x8f", "\xe2\x84\xa2"=>"\xee\x94\xb7",
			//"\xe2\x84\xb9"=>"[\xef\xbd\x89]", "#\xe2\x83\xa3"=>"\xee\x88\x90", "1\xe2\x83\xa3"=>"\xee\x88\x9c", "2\xe2\x83\xa3"=>"\xee\x88\x9d", "3\xe2\x83\xa3"=>"\xee\x88\x9e",
			//"4\xe2\x83\xa3"=>"\xee\x88\x9f", "5\xe2\x83\xa3"=>"\xee\x88\xa0", "6\xe2\x83\xa3"=>"\xee\x88\xa1", "7\xe2\x83\xa3"=>"\xee\x88\xa2", "8\xe2\x83\xa3"=>"\xee\x88\xa3",
			//"9\xe2\x83\xa3"=>"\xee\x88\xa4", "0\xe2\x83\xa3"=>"\xee\x88\xa5", "\xf0\x9f\x94\x9f"=>"[10]", "\xf0\x9f\x93\xb6"=>"\xee\x88\x8b", "\xf0\x9f\x93\xb3"=>"\xee\x89\x90",
			//"\xf0\x9f\x93\xb4"=>"\xee\x89\x91", "\xf0\x9f\x8d\x94"=>"\xee\x84\xa0", "\xf0\x9f\x8d\x99"=>"\xee\x8d\x82", "\xf0\x9f\x8d\xb0"=>"\xee\x81\x86", "\xf0\x9f\x8d\x9c"=>"\xee\x8d\x80",
			//"\xf0\x9f\x8d\x9e"=>"\xee\x8c\xb9", "\xf0\x9f\x8d\xb3"=>"\xee\x85\x87", "\xf0\x9f\x8d\xa6"=>"\xee\x8c\xba", "\xf0\x9f\x8d\x9f"=>"\xee\x8c\xbb", "\xf0\x9f\x8d\xa1"=>"\xee\x8c\xbc",
			//"\xf0\x9f\x8d\x98"=>"\xee\x8c\xbd", "\xf0\x9f\x8d\x9a"=>"\xee\x8c\xbe", "\xf0\x9f\x8d\x9d"=>"\xee\x8c\xbf", "\xf0\x9f\x8d\x9b"=>"\xee\x8d\x81", "\xf0\x9f\x8d\xa2"=>"\xee\x8d\x83",
			//"\xf0\x9f\x8d\xa3"=>"\xee\x8d\x84", "\xf0\x9f\x8d\xb1"=>"\xee\x8d\x8c", "\xf0\x9f\x8d\xb2"=>"\xee\x8d\x8d", "\xf0\x9f\x8d\xa7"=>"\xee\x90\xbf", "\xf0\x9f\x8d\x96"=>"[\xe8\x82\x89]",
			//"\xf0\x9f\x8d\xa5"=>"[\xe3\x81\xaa\xe3\x82\x8b\xe3\x81\xa8]", "\xf0\x9f\x8d\xa0"=>"[\xe3\x82\x84\xe3\x81\x8d\xe3\x81\x84\xe3\x82\x82]", "\xf0\x9f\x8d\x95"=>"[\xe3\x83\x94\xe3\x82\xb6]", "\xf0\x9f\x8d\x97"=>"[\xe3\x83\x81\xe3\x82\xad\xe3\x83\xb3]", "\xf0\x9f\x8d\xa8"=>"[\xe3\x82\xa2\xe3\x82\xa4\xe3\x82\xb9\xe3\x82\xaf\xe3\x83\xaa\xe3\x83\xbc\xe3\x83\xa0]",
			//"\xf0\x9f\x8d\xa9"=>"[\xe3\x83\x89\xe3\x83\xbc\xe3\x83\x8a\xe3\x83\x84]", "\xf0\x9f\x8d\xaa"=>"[\xe3\x82\xaf\xe3\x83\x83\xe3\x82\xad\xe3\x83\xbc]", "\xf0\x9f\x8d\xab"=>"[\xe3\x83\x81\xe3\x83\xa7\xe3\x82\xb3]", "\xf0\x9f\x8d\xac"=>"[\xe3\x82\xad\xe3\x83\xa3\xe3\x83\xb3\xe3\x83\x87\xe3\x82\xa3]", "\xf0\x9f\x8d\xad"=>"[\xe3\x82\xad\xe3\x83\xa3\xe3\x83\xb3\xe3\x83\x87\xe3\x82\xa3]",
			//"\xf0\x9f\x8d\xae"=>"[\xe3\x83\x97\xe3\x83\xaa\xe3\x83\xb3]", "\xf0\x9f\x8d\xaf"=>"[\xe3\x83\x8f\xe3\x83\x81\xe3\x83\x9f\xe3\x83\x84]", "\xf0\x9f\x8d\xa4"=>"[\xe3\x82\xa8\xe3\x83\x93\xe3\x83\x95\xe3\x83\xa9\xe3\x82\xa4]", "\xf0\x9f\x8d\xb4"=>"\xee\x81\x83", "\xe2\x98\x95"=>"\xee\x81\x85",
			//"\xf0\x9f\x8d\xb8"=>"\xee\x81\x84", "\xf0\x9f\x8d\xba"=>"\xee\x81\x87", "\xf0\x9f\x8d\xb5"=>"\xee\x8c\xb8", "\xf0\x9f\x8d\xb6"=>"\xee\x8c\x8b", "\xf0\x9f\x8d\xb7"=>"\xee\x81\x84",
			//"\xf0\x9f\x8d\xbb"=>"\xee\x8c\x8c", "\xf0\x9f\x8d\xb9"=>"\xee\x81\x84", "\xe2\x86\x97"=>"\xee\x88\xb6", "\xe2\x86\x98"=>"\xee\x88\xb8", "\xe2\x86\x96"=>"\xee\x88\xb7",
			//"\xe2\x86\x99"=>"\xee\x88\xb9", "\xe2\xa4\xb4"=>"\xee\x88\xb6", "\xe2\xa4\xb5"=>"\xee\x88\xb8", "\xe2\x86\x94"=>"\xe2\x87\x94", "\xe2\x86\x95"=>"\xe2\x86\x91\xe2\x86\x93",
			//"\xe2\xac\x86"=>"\xee\x88\xb2", "\xe2\xac\x87"=>"\xee\x88\xb3", "\xe2\x9e\xa1"=>"\xee\x88\xb4", "\xe2\xac\x85"=>"\xee\x88\xb5", "\xe2\x96\xb6"=>"\xee\x88\xba",
			//"\xe2\x97\x80"=>"\xee\x88\xbb", "\xe2\x8f\xa9"=>"\xee\x88\xbc", "\xe2\x8f\xaa"=>"\xee\x88\xbd", "\xe2\x8f\xab"=>"\xe2\x96\xb2", "\xe2\x8f\xac"=>"\xe2\x96\xbc",
			//"\xf0\x9f\x94\xba"=>"\xe2\x96\xb2", "\xf0\x9f\x94\xbb"=>"\xe2\x96\xbc", "\xf0\x9f\x94\xbc"=>"\xe2\x96\xb2", "\xf0\x9f\x94\xbd"=>"\xe2\x96\xbc", "\xe2\xad\x95"=>"\xee\x8c\xb2",
			//"\xe2\x9d\x8c"=>"\xee\x8c\xb3", "\xe2\x9d\x8e"=>"\xee\x8c\xb3", "\xe2\x9d\x97"=>"\xee\x80\xa1", "\xe2\x81\x89"=>"\xef\xbc\x81\xef\xbc\x9f", "\xe2\x80\xbc"=>"\xef\xbc\x81\xef\xbc\x81",
			//"\xe2\x9d\x93"=>"\xee\x80\xa0", "\xe2\x9d\x94"=>"\xee\x8c\xb6", "\xe2\x9d\x95"=>"\xee\x8c\xb7", "\xe3\x80\xb0"=>"\xe3\x80\x93", "\xe2\x9e\xb0"=>"\xef\xbd\x9e",
			//"\xe2\x9e\xbf"=>"\xee\x88\x91", "\xe2\x9d\xa4"=>"\xee\x80\xa2", "\xf0\x9f\x92\x93"=>"\xee\x8c\xa7", "\xf0\x9f\x92\x94"=>"\xee\x80\xa3", "\xf0\x9f\x92\x95"=>"\xee\x8c\xa7",
			//"\xf0\x9f\x92\x96"=>"\xee\x8c\xa7", "\xf0\x9f\x92\x97"=>"\xee\x8c\xa8", "\xf0\x9f\x92\x98"=>"\xee\x8c\xa9", "\xf0\x9f\x92\x99"=>"\xee\x8c\xaa", "\xf0\x9f\x92\x9a"=>"\xee\x8c\xab",
			//"\xf0\x9f\x92\x9b"=>"\xee\x8c\xac", "\xf0\x9f\x92\x9c"=>"\xee\x8c\xad", "\xf0\x9f\x92\x9d"=>"\xee\x90\xb7", "\xf0\x9f\x92\x9e"=>"\xee\x8c\xa7", "\xf0\x9f\x92\x9f"=>"\xee\x88\x84",
			//"\xe2\x99\xa5"=>"\xee\x88\x8c", "\xe2\x99\xa0"=>"\xee\x88\x8e", "\xe2\x99\xa6"=>"\xee\x88\x8d", "\xe2\x99\xa3"=>"\xee\x88\x8f", "\xf0\x9f\x9a\xac"=>"\xee\x8c\x8e",
			//"\xf0\x9f\x9a\xad"=>"\xee\x88\x88", "\xe2\x99\xbf"=>"\xee\x88\x8a", "\xf0\x9f\x9a\xa9"=>"[\xe6\x97\x97]", "\xe2\x9a\xa0"=>"\xee\x89\x92", "\xe2\x9b\x94"=>"\xee\x84\xb7",
			//"\xe2\x99\xbb"=>"\xe2\x86\x91\xe2\x86\x93", "\xf0\x9f\x9a\xb2"=>"\xee\x84\xb6", "\xf0\x9f\x9a\xb6"=>"\xee\x88\x81", "\xf0\x9f\x9a\xb9"=>"\xee\x84\xb8", "\xf0\x9f\x9a\xba"=>"\xee\x84\xb9",
			//"\xf0\x9f\x9b\x80"=>"\xee\x84\xbf", "\xf0\x9f\x9a\xbb"=>"\xee\x85\x91", "\xf0\x9f\x9a\xbd"=>"\xee\x85\x80", "\xf0\x9f\x9a\xbe"=>"\xee\x8c\x89", "\xf0\x9f\x9a\xbc"=>"\xee\x84\xba",
			//"\xf0\x9f\x9a\xaa"=>"[\xe3\x83\x89\xe3\x82\xa2]", "\xf0\x9f\x9a\xab"=>"[\xe7\xa6\x81\xe6\xad\xa2]", "\xe2\x9c\x94"=>"[\xe3\x83\x81\xe3\x82\xa7\xe3\x83\x83\xe3\x82\xaf\xe3\x83\x9e\xe3\x83\xbc\xe3\x82\xaf]", "\xf0\x9f\x86\x91"=>"[CL]", "\xf0\x9f\x86\x92"=>"\xee\x88\x94",
			//"\xf0\x9f\x86\x93"=>"[FREE]", "\xf0\x9f\x86\x94"=>"\xee\x88\xa9", "\xf0\x9f\x86\x95"=>"\xee\x88\x92", "\xf0\x9f\x86\x96"=>"[NG]", "\xf0\x9f\x86\x97"=>"\xee\x89\x8d",
			//"\xf0\x9f\x86\x98"=>"[SOS]", "\xf0\x9f\x86\x99"=>"\xee\x88\x93", "\xf0\x9f\x86\x9a"=>"\xee\x84\xae", "\xf0\x9f\x88\x81"=>"\xee\x88\x83", "\xf0\x9f\x88\x82"=>"\xee\x88\xa8",
			//"\xf0\x9f\x88\xb2"=>"[\xe7\xa6\x81]", "\xf0\x9f\x88\xb3"=>"\xee\x88\xab", "\xf0\x9f\x88\xb4"=>"[\xe5\x90\x88]", "\xf0\x9f\x88\xb5"=>"\xee\x88\xaa", "\xf0\x9f\x88\xb6"=>"\xee\x88\x95",
			//"\xf0\x9f\x88\x9a"=>"\xee\x88\x96", "\xf0\x9f\x88\xb7"=>"\xee\x88\x97", "\xf0\x9f\x88\xb8"=>"\xee\x88\x98", "\xf0\x9f\x88\xb9"=>"\xee\x88\xa7", "\xf0\x9f\x88\xaf"=>"\xee\x88\xac",
			//"\xf0\x9f\x88\xba"=>"\xee\x88\xad", "\xe3\x8a\x99"=>"\xee\x8c\x95", "\xe3\x8a\x97"=>"\xee\x8c\x8d", "\xf0\x9f\x89\x90"=>"\xee\x88\xa6", "\xf0\x9f\x89\x91"=>"[\xe5\x8f\xaf]",
			//"\xe2\x9e\x95"=>"[\xef\xbc\x8b]", "\xe2\x9e\x96"=>"[\xef\xbc\x8d]", "\xe2\x9c\x96"=>"\xee\x8c\xb3", "\xe2\x9e\x97"=>"[\xc3\xb7]", "\xf0\x9f\x92\xa0"=>"\xe3\x80\x93",
			//"\xf0\x9f\x92\xa1"=>"\xee\x84\x8f", "\xf0\x9f\x92\xa2"=>"\xee\x8c\xb4", "\xf0\x9f\x92\xa3"=>"\xee\x8c\x91", "\xf0\x9f\x92\xa4"=>"\xee\x84\xbc", "\xf0\x9f\x92\xa5"=>"[\xe3\x83\x89\xe3\x83\xb3\xe3\x83\x83]",
			//"\xf0\x9f\x92\xa6"=>"\xee\x8c\xb1", "\xf0\x9f\x92\xa7"=>"\xee\x8c\xb1", "\xf0\x9f\x92\xa8"=>"\xee\x8c\xb0", "\xf0\x9f\x92\xa9"=>"\xee\x81\x9a", "\xf0\x9f\x92\xaa"=>"\xee\x85\x8c",
			//"\xf0\x9f\x92\xab"=>"\xee\x90\x87", "\xf0\x9f\x92\xac"=>"[\xe3\x83\x95\xe3\x82\xad\xe3\x83\x80\xe3\x82\xb7]", "\xe2\x9c\xa8"=>"\xee\x8c\xae", "\xe2\x9c\xb4"=>"\xee\x88\x85", "\xe2\x9c\xb3"=>"\xee\x88\x86",
			//"\xe2\x9a\xaa"=>"\xee\x88\x99", "\xe2\x9a\xab"=>"\xee\x88\x99", "\xf0\x9f\x94\xb4"=>"\xee\x88\x99", "\xf0\x9f\x94\xb5"=>"\xee\x88\x9a", "\xf0\x9f\x94\xb2"=>"\xee\x88\x9a",
			//"\xf0\x9f\x94\xb3"=>"\xee\x88\x9b", "\xe2\xad\x90"=>"\xee\x8c\xaf", "\xe2\xac\x9c"=>"\xee\x88\x9b", "\xe2\xac\x9b"=>"\xee\x88\x9a", "\xe2\x96\xab"=>"\xee\x88\x9b",
			//"\xe2\x96\xaa"=>"\xee\x88\x9a", "\xe2\x97\xbd"=>"\xee\x88\x9b", "\xe2\x97\xbe"=>"\xee\x88\x9a", "\xe2\x97\xbb"=>"\xee\x88\x9b", "\xe2\x97\xbc"=>"\xee\x88\x9a",
			//"\xf0\x9f\x94\xb6"=>"\xee\x88\x9b", "\xf0\x9f\x94\xb7"=>"\xee\x88\x9b", "\xf0\x9f\x94\xb8"=>"\xee\x88\x9b", "\xf0\x9f\x94\xb9"=>"\xee\x88\x9b", "\xe2\x9d\x87"=>"\xee\x8c\xae",
			//"\xf0\x9f\x92\xae"=>"[\xe8\x8a\xb1\xe4\xb8\xb8]", "\xf0\x9f\x92\xaf"=>"[100\xe7\x82\xb9]", "\xe2\x86\xa9"=>"\xe2\x86\x90\xe2\x94\x98", "\xe2\x86\xaa"=>"\xe2\x94\x94\xe2\x86\x92", "\xf0\x9f\x94\x83"=>"\xe2\x86\x91\xe2\x86\x93",
			//"\xf0\x9f\x94\x8a"=>"\xee\x85\x81", "\xf0\x9f\x94\x8b"=>"[\xe9\x9b\xbb\xe6\xb1\xa0]", "\xf0\x9f\x94\x8c"=>"[\xe3\x82\xb3\xe3\x83\xb3\xe3\x82\xbb\xe3\x83\xb3\xe3\x83\x88]", "\xf0\x9f\x94\x8d"=>"\xee\x84\x94", "\xf0\x9f\x94\x8e"=>"\xee\x84\x94",
			//"\xf0\x9f\x94\x92"=>"\xee\x85\x84", "\xf0\x9f\x94\x93"=>"\xee\x85\x85", "\xf0\x9f\x94\x8f"=>"\xee\x85\x84", "\xf0\x9f\x94\x90"=>"\xee\x85\x84", "\xf0\x9f\x94\x91"=>"\xee\x80\xbf",
			//"\xf0\x9f\x94\x94"=>"\xee\x8c\xa5", "\xe2\x98\x91"=>"[\xe3\x83\x81\xe3\x82\xa7\xe3\x83\x83\xe3\x82\xaf\xe3\x83\x9e\xe3\x83\xbc\xe3\x82\xaf]", "\xf0\x9f\x94\x98"=>"[\xe3\x83\xa9\xe3\x82\xb8\xe3\x82\xaa\xe3\x83\x9c\xe3\x82\xbf\xe3\x83\xb3]", "\xf0\x9f\x94\x96"=>"[\xe3\x83\x96\xe3\x83\x83\xe3\x82\xaf\xe3\x83\x9e\xe3\x83\xbc\xe3\x82\xaf]", "\xf0\x9f\x94\x97"=>"[\xe3\x83\xaa\xe3\x83\xb3\xe3\x82\xaf]",
			//"\xf0\x9f\x94\x99"=>"\xee\x88\xb5", "\xf0\x9f\x94\x9a"=>"[end]", "\xf0\x9f\x94\x9b"=>"[ON]", "\xf0\x9f\x94\x9c"=>"[SOON]", "\xf0\x9f\x94\x9d"=>"\xee\x89\x8c",
			//"\xe2\x9c\x85"=>"[\xe3\x83\x81\xe3\x82\xa7\xe3\x83\x83\xe3\x82\xaf\xe3\x83\x9e\xe3\x83\xbc\xe3\x82\xaf]", "\xe2\x9c\x8a"=>"\xee\x80\x90", "\xe2\x9c\x8b"=>"\xee\x80\x92", "\xe2\x9c\x8c"=>"\xee\x80\x91", "\xf0\x9f\x91\x8a"=>"\xee\x80\x8d",
			//"\xf0\x9f\x91\x8d"=>"\xee\x80\x8e", "\xe2\x98\x9d"=>"\xee\x80\x8f", "\xf0\x9f\x91\x86"=>"\xee\x88\xae", "\xf0\x9f\x91\x87"=>"\xee\x88\xaf", "\xf0\x9f\x91\x88"=>"\xee\x88\xb0",
			//"\xf0\x9f\x91\x89"=>"\xee\x88\xb1", "\xf0\x9f\x91\x8b"=>"\xee\x90\x9e", "\xf0\x9f\x91\x8f"=>"\xee\x90\x9f", "\xf0\x9f\x91\x8c"=>"\xee\x90\xa0", "\xf0\x9f\x91\x8e"=>"\xee\x90\xa1",
			//"\xf0\x9f\x91\x90"=>"\xee\x90\xa2",
   //     ),



    class Emoji
    {
    }
}
