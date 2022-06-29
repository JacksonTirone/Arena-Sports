import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-pacote-cadastro',
  templateUrl: './pacote-cadastro.component.html',
  styleUrls: ['./pacote-cadastro.component.css']
})
export class PacoteCadastroComponent implements OnInit {

  clicado = false;
  acao: String
  data = { churrasqueiraPacoteId: 0, descricao: "", valor: "" }
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


        let envelope = {'churrasqueiraPacoteId': this.data.churrasqueiraPacoteId, 'descricao': this.data.descricao, 'valor': this.data.valor, 'ativo': true };
        this.service.postRequestDefault('Churrasqueira/CadastrarAtualizarPacoteChurrasqueira', envelope).subscribe(data => {
          if (data.success) {
            this.mensagem.sucesso('Pacote cadastrado com Sucesso');
            this.router.navigate(['/pacote-pesquisa']);
          } else {
            this.mensagem.erro(data.message);
          }
          aguarde.close();
        }, error => {
          this.mensagem.erro('Não foi possível executar a ação: ' + error.error.message);
          aguarde.close();
        });
      }
    });
  }
  
  voltar() {
    this.router.navigate(["/pacote-pesquisa"]);
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
