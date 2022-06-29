import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-quadra-cadastro',
  templateUrl: './quadra-cadastro.component.html',
  styleUrls: ['./quadra-cadastro.component.css']
})
export class QuadraCadastroComponent implements OnInit {
  acao: string
  constructor(private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, private router: Router) { }

  clicado = false;
  data = { quadraId: 0,  descricao: "", pisoId: 1, esporteId: 1, coberta: true, status: true };
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

        this.service.postRequestDefault('Quadra/CadastrarAtualizarQuadra', this.data)
          .subscribe(data => {
              if (data.success) {
                this.mensagem.sucesso("Quadra cadastrado com Sucesso");
                this.router.navigate(["/quadra-pesquisa"]);
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
    this.router.navigate(["/quadra-pesquisa"]);
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
    return of(erro);
  }

  vazio(valor: any) {
    return (valor === undefined || valor.length < 1);
  }
}
