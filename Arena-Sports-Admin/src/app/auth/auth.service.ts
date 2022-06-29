import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Login } from '../model/login';
import { ResponseDefault } from '../model/responseDefault';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router: Router) { }
  
  public login(login: Login) {
    let pacote = { "email": login.email, "password": login.password };
    return this.http.post<ResponseDefault>(environment.apiPrefix + 'Auth/login', pacote);
  }

  public isLoggedIn() { 
    return localStorage.getItem('token') !== null;
  }

  public logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.router.navigate(["/login"]);
  }

  public CarregarDefaults() {
    this.http.get<ResponseDefault>(environment.apiPrefix + 'Quadra/GetAllOpcionais')
              .subscribe(
                  data => {
                    localStorage.setItem('quadraOpcionais', JSON.stringify(data.data));
                  });

    this.http.get<ResponseDefault>(environment.apiPrefix + 'Churrasqueira/GetAllPacote')
              .subscribe(
                  data => {
                    localStorage.setItem('churrasqueiraPacotes', JSON.stringify(data.data));
                  });
  }
}
