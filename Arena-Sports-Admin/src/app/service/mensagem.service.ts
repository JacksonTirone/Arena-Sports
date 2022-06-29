import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class MensagemService {

  constructor(private toastr: ToastrService) { }

  sucesso(mensagem: string) {    
    this.toastr.success(mensagem, 'Sucesso', {
      timeOut: 3000
    });
  }

  erro(mensagem: string) {
    this.toastr.error(mensagem, 'Ops', {
      timeOut: 3000
    });
  }

  info(mensagem: string) {
    this.toastr.info(mensagem, '', {
      timeOut: 3000
    });
  }
}
