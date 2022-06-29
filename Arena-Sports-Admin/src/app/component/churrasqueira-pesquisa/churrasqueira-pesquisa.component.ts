import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-churrasqueira-pesquisa',
  templateUrl: './churrasqueira-pesquisa.component.html',
  styleUrls: ['./churrasqueira-pesquisa.component.css']
})
export class ChurrasqueiraPesquisaComponent implements OnInit {

  displayedColumns = ["churrasqueira", "itens", "acao"]
  dataSource = []

  constructor(private router: Router, private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, public dialog: MatDialog) { }

  ngOnInit() {
    this.find()
  }

  find() {
    const aguarde = this.aguardeService.aguarde();

    this.service.getDot('Churrasqueira/GetAll').subscribe(data => {
      if (data.success) {
        this.dataSource = data.data;
      } else {
        this.mensagem.erro(data.message);
      }
      aguarde.close();
    }, error => {
      this.mensagem.erro("Não foi possível executar a ação: " + error);
      aguarde.close();
    });
  }

  cadastrar() {
    this.router.navigate(["/churrasqueira-cadastro"]);
  }

  alterar(valor) {
    localStorage.setItem("valor", JSON.stringify(valor));
    this.router.navigate(["/churrasqueira-cadastro/"]);
  }

  openDialogExcluir(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Excluir Churrasqueira ' + selecionado.descricao
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Churrasqueira/DeleteChurrasqueira', selecionado.churrasqueiraId).subscribe(data => {
          this.mensagem.sucesso("Churrasqueira removida com Sucesso");
          this.find();
        });
      }
    });
  }
}