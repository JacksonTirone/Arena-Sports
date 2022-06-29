import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AguardeService } from 'src/app/service/aguarde.service';
import { ApiService } from 'src/app/service/api.service';
import { MensagemService } from 'src/app/service/mensagem.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-adicionar-cliente',
  templateUrl: './adicionar-cliente.component.html',
  styleUrls: ['./adicionar-cliente.component.css']
})

export class AdicionarClienteComponent implements OnInit {
  
  cadClienteForm: FormGroup;

  get formControls() { return this.cadClienteForm.controls; }
  get cpfInput() { return this.cadClienteForm.get('cpf'); }
  get nomeInput() { return this.cadClienteForm.get('nome'); }

  constructor(
    public dialogRef: MatDialogRef<AdicionarClienteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.cadClienteForm = new FormGroup({
      cpf: new FormControl('', [Validators.required, Validators.minLength(11)]),
      nome: new FormControl('', [Validators.required, Validators.pattern('^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ\s ]+$')])
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  salvarCliente(): void {
    if(this.cadClienteForm.invalid) {
      return;
    }
    
    const aguarde = this.aguardeService.aguarde();
    this.service.postRequestDefault('Usuario/CadastrarClienteSimples', this.cadClienteForm.value)
      .subscribe(
        data => {
          aguarde.close();
          this.dialogRef.close(data.data);
        },
        error => {
          this.mensagem.erro(error.error.message);
          aguarde.close();
        });
  }
}
