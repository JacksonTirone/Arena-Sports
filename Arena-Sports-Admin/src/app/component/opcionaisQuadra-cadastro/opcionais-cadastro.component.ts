import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-opcionais-cadastro',
  templateUrl: './opcionais-cadastro.component.html',
  styleUrls: ['./opcionais-cadastro.component.css']
})
export class OpcionaisCadastroComponent implements OnInit {

  clicado = false;
  acao: string;
  data = { quadraItemOpcionalId: 0, descricao: "", valor: "" }

  constructor(private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem("valor")) {
      this.acao = "Alterar";
      this.data = JSON.parse(localStorage.getItem("valor"));
      localStorage.removeItem("valor");
    } else {
      this.acao = "Cadastrar";
    }
  }

  salvar() {
    this.verificaErros().subscribe(erro => {
      if (!erro) {
        const aguarde = this.aguardeService.aguarde();

        let envelope = {"quadraItemOpcionalId": this.data.quadraItemOpcionalId, "descricao": this.data.descricao, "valor": this.data.valor };
        this.service.postRequestDefault('Quadra/CadastrarAtualizarOpcionalQuadra', envelope).subscribe(data => {
          if (data.success) {
            this.mensagem.sucesso("Opcional cadastrado com Sucesso");
            this.router.navigate(["/opcionais-pesquisa"]);
          } else {
            this.mensagem.erro(data.message);
          }
          aguarde.close();
        }, error => {
          this.mensagem.erro("Não foi possível executar a ação : " + error.error.message);
          aguarde.close();
        });

      }
    });
  }

  voltar() {
    this.router.navigate(["/opcionais-pesquisa"]);
  }

  vazioClicado(valor) {
    if (this.clicado) {
      return this.vazio(valor);
    }
  }

  verificaErros(): Observable<boolean> {
    this.clicado = true;
    let erro = false;

    if (this.vazio(this.data.descricao)) {
      erro = true;
    }
    if (this.vazio(this.data.valor)) {
      erro = true;
    }
    return of(erro);
  }

  vazio(valor: any) {
    return (valor === undefined || valor.length < 1);
  }
}
