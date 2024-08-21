let baseUrl="/api";
if(process.env.NODE_ENV!="production"){
     baseUrl = 'http://autodotua.top:12336/api';
}

console.log("baseUrl="+baseUrl)
export default baseUrl;