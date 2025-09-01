parser grammar MigxnLanguage;

import MigxnStmt;

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

language
    : statement;
