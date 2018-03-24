export interface ApiResult<T = any> {
  result: boolean;
  errors: string[];
  data: T;
}
