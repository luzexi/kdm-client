

Readme before use network framework

1, 整体协议

		// request of protocol
		{
		    u:uid, //account id
		    t:request_count, // total request count
		    s:session_id, // session id
		    d:[], // request data
		    k:md5_all_of_before, //plus all of before data and a key
		}

		d:[ {
		      o:sequence_id, // sequence id for request
		      c:command id or command name, //module name or cammand name or some thing else like that.
		      p:param, // param looks like this => { gold:12, id_list:[1,2,3], .... }
		    
			},
			{ the same as before but different sequence_id and maybe different command id },
			{ the same as before }
		]

		// response of protocal
		{
		    r:result,
		    m:message
		    d: [ 
		          {response of data},
		          {response of data},
		            .....
		        ]
		}


		// detail of response data
		{
		    o:sequence_id, // sequence id for request
		    d:data of response, // data of response
		}



2, 每个系统的网络接口，可以自己写一个文件，用public partial class GameNetwork，来拆分GameNetwork。

3, 写请求和回调时，来回的 request 是同一个实例，请求的数据变量，可以在回调时直接使用。

4，协议ErrorCode客户端统一处理方法如下：
第一步。问服务器要各个错误码对应的含义；
第二步。将各个错误码含义配置到配置文件中（text.xls）,按如下格式（ERROR_协议号_错误码号）配置主键；
第三步。在协议response的时候，统一调用如下代码：
if (request.mError != 0)
{
       TextManager.instance.ShowErrorCodeMsg(request.cmdId, request.mError);
       return;
}