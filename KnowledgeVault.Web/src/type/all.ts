import Cookies from 'js-cookie'


export interface OptionInt {
  value: number,
  label: string
}

export class InitDataAll {
  // 身份状态 true：管理员， false: 游客
  identityID: boolean = Cookies.get("token")!=null ? true : false
  options: OptionInt[] = [
    {
      value: 1,
      label: '论文',
    },
    {
      value: 2,
      label: '专利',
    },
    {
      value: 3,
      label: '软著',
    },
    {
      value: 4,
      label: '奖项',
    },
    {
      value: 5,
      label: '项目',
    },
  ];
  selectType: null | number = null;  // 选中新增的成果类型
  selectTypeTitle = "";  // 选中新增的成果类型名称
  drawer = false; //抽屉状态
}