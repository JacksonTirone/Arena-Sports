import { Component, OnInit } from '@angular/core';
import { AguardeService } from 'src/app/service/aguarde.service';
import { ApiService } from 'src/app/service/api.service';
import { MensagemService } from 'src/app/service/mensagem.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-churrasqueira-cadastro',
  templateUrl: './churrasqueira-cadastro.component.html',
  styleUrls: ['./churrasqueira-cadastro.component.css']
})
export class ChurrasqueiraCadastroComponent implements OnInit {

  acao: string;
  data = { descricao: "", descricaoItens: "", churrasqueiraId: 0 };
  clicado = false;
  constructor(private router: Router, private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService) { }

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
      if(!erro){
        const aguarde = this.aguardeService.aguarde();
    
        let envelope = { "churrasqueiraId": this.data.churrasqueiraId, "descricao": this.data.descricao, "descricaoItens": this.data.descricaoItens };
        this.service.postRequestDefault('Churrasqueira/CadastrarAtualizarChurrasqueira', envelope).subscribe(data => {
          if (data.success) {
            this.mensagem.sucesso("Churrasqueira cadastrada com Sucesso");
            this.router.navigate(["/churrasqueira-pesquisa"]);
          } else {
            this.mensagem.erro(data.message);
          }
          aguarde.close();
        }, error => {
          this.mensagem.erro("Não foi possível executar a ação : " + error.error.message);
          aguarde.close();
        });
      }
    })
  }

  voltar() {
    this.router.navigate(["/churrasqueira-pesquisa"]);
  }

  vazioClicado(valor) {
    if (this.clicado) {
      return this.vazio(valor);
    }
  }

  verificaErros(): Observable<boolean> {
    this.clicado = true;
    let erro = false;

    if(this.vazio(this.data.descricao)){
      erro = true;
    }
    if(this.vazio(this.data.descricaoItens)){
      erro = true;
    }

    return of(erro);
  }

  vazio(valor: any) {
    return (valor === undefined || valor.length < 1);
  }
}
