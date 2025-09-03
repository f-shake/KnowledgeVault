let baseUrl="/api";
if(process.env.NODE_ENV!="production"){
     baseUrl = 'http://h280ylx.dongtaiyuming.net/api';
     baseUrl = 'http://localhost:5289';
}

console.log("baseUrl="+baseUrl)
export default baseUrl;