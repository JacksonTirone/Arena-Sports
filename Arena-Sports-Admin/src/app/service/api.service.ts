import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Cliente } from '../model/cliente';
import { ResponseDefault } from '../model/responseDefault';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  baseUrl = environment.apiPrefix;
  /*DOTNET*/

  postRequestDefault(base: string, parametro: any) {
    return this.http.post<ResponseDefault>(environment.apiPrefix + base, parametro
    );
  }

  getDot(base: string) {
    return this.http.get<ResponseDefault>(environment.apiPrefix + base);
  } fcd 

  getClientesTop10(cpfNome: string) {
    return this.http.get<Cliente[]>(environment.apiPrefix + 'Usuario/GetTop10Clientes' + '?cpfNome=' + cpfNome);
  }

  excluir(base: string, valor: any) {
    return this.http.delete<ResponseDefault>(environment.apiPrefix + base + "?id=" + valor);
  }
}
