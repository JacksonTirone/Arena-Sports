import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-opcionais-pesquisa',
  templateUrl: './opcionais-pesquisa.component.html',
  styleUrls: ['./opcionais-pesquisa.component.css']
})
export class OpcionaisPesquisaComponent implements OnInit {

  displayedColumns = ["descricao", "itens","acao"]
  dataSource = []

  constructor(private router: Router, private aguardeService: AguardeService,private service: ApiService, private  mensagem: MensagemService, private dialog: MatDialog) { }

  ngOnInit() {
    this.find();
  }

  find() {
    const aguarde = this.aguardeService.aguarde();

    this.service.getDot('Quadra/GetAllOpcionais').subscribe(data => {
      if (data.success) {
        this.dataSource = data.data;
        localStorage.setItem('quadraOpcionais', JSON.stringify(data.data));
      } else {
        this.mensagem.erro(data.message);
      }
      aguarde.close();
    }, error => {
      this.mensagem.erro("Não foi possível executar a ação: " + error);
      aguarde.close();
    });
  }

  alterar(valor) {
    localStorage.setItem("valor", JSON.stringify(valor));
    this.router.navigate(["/opcionais-cadastro/"]);
  }

  cadastrar() {
    this.router.navigate(["/opcionais-cadastro"]);
  }

  openDialogExcluir(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Excluir Opcional ' + selecionado.descricao
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Quadra/DeleteOpcionalQuadra',  selecionado.quadraItemOpcionalId).subscribe(data => {
          this.mensagem.sucesso("Opcional removido com Sucesso");
          this.find();
        });
      }
    });
  }
}
